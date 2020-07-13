using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Synonms.Versioning.Core;
using Synonms.Versioning.Core.Extensions;

namespace Synonms.Versioning.Swashbuckle
{
    public class VersionableDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths = GetPrunedPaths(swaggerDoc.Paths, swaggerDoc.Info.Version);
            swaggerDoc.Components.Schemas = GetPrunedSchemas(swaggerDoc.Components.Schemas, swaggerDoc.Info.Version);
        }

        private static OpenApiPaths GetPrunedPaths(OpenApiPaths paths, string swaggerDocVersion)
        {
            var prunedPaths = new OpenApiPaths();

            foreach (var openApiPath in paths)
            {
                if (openApiPath.Key.Contains(swaggerDocVersion))
                {
                    prunedPaths.Add(openApiPath.Key, openApiPath.Value);
                }
            }

            return prunedPaths;
        }

        private static IDictionary<string, OpenApiSchema> GetPrunedSchemas(IDictionary<string, OpenApiSchema> schemas, string swaggerDocVersion)
        {
            var version = new Version(swaggerDocVersion);
            var prunedSchemas = new Dictionary<string, OpenApiSchema>();

            foreach (var schema in schemas.Where(s => s.Value?.Description != null))
            {
                var objectData = schema.Value.Description;

                // VersionableSwaggerSchemaAttribute packs data as "@[ClassName]@Description
                var regEx = new Regex(@"^@\[(?'className'[\s\S]+)\]@(?'description'[\s\S]*)$");
                var matches = regEx.Matches(objectData);

                if (matches.Count == 0)
                {
                    // Not decorated with VersionableSwaggerSchemaAttribute so pass through untouched
                    prunedSchemas.Add(schema.Key, schema.Value);
                    continue;
                }

                var objectName = matches[0].Groups["className"].Value;
                schema.Value.Description = matches[0].Groups["description"].Value;

                if (string.IsNullOrWhiteSpace(objectName))
                {
                    // Can't determine Versionable object so pass through untouched
                    prunedSchemas.Add(schema.Key, schema.Value);
                    continue;
                }

                var objectType = Type.GetType(objectName);

                if (objectType == null || !objectType.IsClass)
                {
                    // Not Versionable so pass through untouched
                    prunedSchemas.Add(schema.Key, schema.Value);
                    continue;
                }

                var objectVersionHistory = objectType.GetVersionHistory();

                if (!objectVersionHistory.IsApplicableAtVersion(version))
                {
                    // Entire object not applicable - discard
                    continue;
                }

                var excludedProperties = new List<string>();

                foreach(var property in schema.Value.Properties)
                {
                    var propertyInfo = objectType.GetPropertyBySerialisationName(property.Key);

                    if (propertyInfo == null) continue;

                    var propertyVersionHistory = propertyInfo.GetVersionHistory();
                    var applicableVersionHistory = VersionHistory.Merge(propertyVersionHistory, objectVersionHistory);

                    if (!applicableVersionHistory.IsApplicableAtVersion(version))
                    {
                        excludedProperties.Add(property.Key);
                    }
                }

                foreach (var excludedProperty in excludedProperties.Where(excludedProperty => schema.Value.Properties.ContainsKey(excludedProperty)))
                {
                    schema.Value.Properties.Remove(excludedProperty);
                }

                prunedSchemas.Add(schema.Key, schema.Value);
            }

            return prunedSchemas;
        }
    }
}

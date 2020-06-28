using System;
using System.Collections.Generic;
using System.Linq;
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
            if (swaggerDoc.Components.Schemas.Count == 0) return;

            var version = new Version(swaggerDoc.Info.Version);

            foreach (var schema in swaggerDoc.Components.Schemas)
            {
                var objectName = schema.Value.Description;
                var objectType = Type.GetType(objectName);

                if (objectType == null) continue;
                if (!objectType.IsClass) continue;
                if (!objectType.GetInterfaces().Contains(typeof(IVersionable))) continue;

                var objectVersionHistory = objectType.GetVersionHistory();
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
            }
        }
    }
}

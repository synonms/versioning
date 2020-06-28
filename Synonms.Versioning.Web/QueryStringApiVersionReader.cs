using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Synonms.Versioning.Core;

namespace Synonms.Versioning.Web
{
    public class QueryStringApiVersionReader : IApiVersionReader
    {
        private const string DefaultQueryParameterName = "api-version";
        private readonly ICollection<string> _parameterNames;

        public QueryStringApiVersionReader()
        {
            _parameterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { DefaultQueryParameterName };
        }

        public QueryStringApiVersionReader(ICollection<string> parameterNames)
        {
            _parameterNames = parameterNames?.Count > 0 ? parameterNames : new HashSet<string>(StringComparer.OrdinalIgnoreCase) { DefaultQueryParameterName };
        }

        public Version Read(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var rawApiVersions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var parameterName in _parameterNames)
            {
                var values = context.Request.Query[parameterName];

                foreach (var value in values)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        rawApiVersions.Add(value);
                    }
                }
            }

            if (rawApiVersions.Count > 1) throw new VersionException($"Conflicting API versions found in query string: {string.Join(",", rawApiVersions)}");

            return Version.TryParse(rawApiVersions.FirstOrDefault(), out var apiVersion) ? apiVersion : default;
        }
    }
}

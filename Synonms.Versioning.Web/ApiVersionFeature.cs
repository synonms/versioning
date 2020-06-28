using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Synonms.Versioning.Core;
using Synonms.Versioning.Core.Extensions;

namespace Synonms.Versioning.Web
{
    public class ApiVersionFeature : IApiVersionFeature
    {
        private readonly HttpContext _context;
        private readonly ICollection<IApiVersionReader> _readers;

        public ApiVersionFeature(HttpContext context, ICollection<IApiVersionReader> readers)
        {
            _context = context;
            _readers = readers;
        }

        public Version GetRequestedVersion()
        {
            var apiVersions = new HashSet<Version>();

            foreach (var apiVersionReader in _readers)
            {
                var apiVersion = apiVersionReader.Read(_context);

                if (apiVersion != null && !apiVersion.IsUnspecified())
                {
                    apiVersions.Add(apiVersion);
                }
            }

            if (apiVersions.Count > 1) throw new VersionException($"Conflicting API versions requested: {string.Join(",", apiVersions.Select(Version => Version.ToString()))}");

            return apiVersions.FirstOrDefault();
        }
    }
}

using System;
using Microsoft.AspNetCore.Http;

namespace Synonms.Versioning.Web
{
    public class UrlApiVersionReader : IApiVersionReader
    {
        public Version Read(HttpContext context)
        {
            var path = context.Request.Path;
            if (!path.HasValue) return default;

            var segments = path.Value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length > 0 && Version.TryParse(segments[0], out var segment0ApiVersion)) return segment0ApiVersion;
            if (segments.Length > 0 && Version.TryParse(segments[1], out var segment1ApiVersion)) return segment1ApiVersion;

            return default;
        }
    }
}

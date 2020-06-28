using System;
using Microsoft.AspNetCore.Http;

namespace Synonms.Versioning.Web
{
    public static class HttpContextExtensions
    {
        public static Version GetRequestedVersion(this HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var feature = context.Features.Get<IApiVersionFeature>();

            return feature?.GetRequestedVersion();
        }
    }
}

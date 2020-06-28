using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Synonms.Versioning.Web
{
    public class ApiVersionMiddleware : IMiddleware
    {
        private readonly ICollection<IApiVersionReader> _apiVersionReaders;

        public ApiVersionMiddleware()
        {
            _apiVersionReaders = new List<IApiVersionReader>
            {
                new UrlApiVersionReader(),
                new QueryStringApiVersionReader()
            };
        }

        public ApiVersionMiddleware(ICollection<IApiVersionReader> apiVersionReaders)
        {
            _apiVersionReaders = apiVersionReaders?.Count > 0 ? apiVersionReaders : new List<IApiVersionReader>
            {
                new UrlApiVersionReader(),
                new QueryStringApiVersionReader()
            };
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Features.Set<IApiVersionFeature>(new ApiVersionFeature(context, _apiVersionReaders));

            await next(context);
        }
    }
}

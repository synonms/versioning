using System;
using Microsoft.AspNetCore.Http;

namespace Synonms.Versioning.Web
{
    public interface IApiVersionReader
    {
        Version Read(HttpContext context);
    }
}

using System;

namespace Synonms.Versioning.Web
{
    public interface IApiVersionFeature
    {
        Version GetRequestedVersion();
    }
}

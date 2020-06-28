using System;

namespace Synonms.Versioning.Core.Extensions
{
    public static class VersionExtensions
    {
        public static bool IsUnspecified(this Version version)
        {
            return version.Major<= 0 && version.Minor < 0 && version.Build <= 0 && version.Revision <= 0;
        }
    }
}

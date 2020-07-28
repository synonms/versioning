using System;

namespace Synonms.Versioning.Core.Extensions
{
    public static class VersionExtensions
    {
        public static bool IsUnspecified(this Version version)
        {
            return version.Major <= 0 && version.Minor <= 0 && version.Build <= 0 && version.Revision <= 0;
        }

        public static string ToMinifiedString(this Version version, bool isPrefixed = false)
        {
            var output = string.Empty;
            var isIncrementDetected = false;

            if (version.Revision > 0)
            {
                isIncrementDetected = true;
                output = $".{version.Revision}";
            }

            if (isIncrementDetected || version.Build > 0)
            {
                isIncrementDetected = true;
                output = $".{version.Build}{output}";
            }

            if (isIncrementDetected || version.Minor > 0)
            {
                output = $".{version.Minor}{output}";
            }

            output = $"{version.Major}{output}";

            return isPrefixed ? $"v{output}" : output;
        }
    }
}

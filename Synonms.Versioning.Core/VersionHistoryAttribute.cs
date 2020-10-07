using System;

namespace Synonms.Versioning.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class VersionHistoryAttribute : Attribute
    {
        public int IntroducedMajorVersion { get; set; } = 0;
        public int IntroducedMinorVersion { get; set; } = 0;
        public int DeprecatedMajorVersion { get; set; } = 0;
        public int DeprecatedMinorVersion { get; set; } = 0;
    }
}

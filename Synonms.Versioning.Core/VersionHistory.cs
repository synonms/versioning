using System;
using Synonms.Versioning.Core.Extensions;

namespace Synonms.Versioning.Core
{
    public class VersionHistory
    {
        public VersionHistory(Version introduced, Version deprecated = null)
        {
            Introduced = introduced;
            Deprecated = deprecated ?? new Version();
        }

        public Version Introduced { get; }
        public Version Deprecated { get; }

        public bool IsDeprecated => Deprecated != null && Deprecated.IsUnspecified();

        public bool IsApplicableAtVersion(Version version)
        {
            return version >= Introduced && (!IsDeprecated || version < Deprecated);
        }

        public static VersionHistory Merge(VersionHistory primary, VersionHistory secondary)
        {
            return new VersionHistory(
                primary.Introduced.IsUnspecified() ? secondary.Introduced : primary.Introduced,
                primary.Deprecated.IsUnspecified() ? secondary.Deprecated : primary.Deprecated);
        }
    }
}

using Synonms.Versioning.Core;
using Synonms.Versioning.Swashbuckle;

namespace Synonms.Versioning.AspNetCore.Example.Models
{
    [VersionableSwaggerSchema(typeof(Resource))]
    public class Resource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [VersionHistory(DeprecatedMajorVersion = 2, DeprecatedMinorVersion = 0)]
        public string SuperOldDeprecatedThingy { get; set; }

        [VersionHistory(IntroducedMajorVersion = 2, IntroducedMinorVersion = 0)]
        public string BrandNewThingy { get; set; }
    }
}

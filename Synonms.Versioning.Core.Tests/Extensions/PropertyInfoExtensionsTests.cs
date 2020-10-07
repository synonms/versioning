using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Synonms.Versioning.Core.Extensions;
using Xunit;

namespace Synonms.Versioning.Core.Tests.Extensions
{
    public class TestClass
    {
        public int LongRunningProperty { get; set; }

        [VersionHistory(DeprecatedMajorVersion = 2, DeprecatedMinorVersion = 1)]
        public int OldProperty { get; set; }

        [JsonPropertyName("myNewName")]
        [VersionHistory(IntroducedMajorVersion = 2, IntroducedMinorVersion = 0)]
        public int NewProperty { get; set; }
    }

    public class PropertyInfoExtensionsTests
    {
        [Fact]
        public void ApplyAttribute_GivenAttributePresent_ThenExecutesAction()
        {
            var property = typeof(TestClass).GetProperty("OldProperty");

            var deprecatedMajorVersion = 0;
            var deprecatedMinorVersion = 0;

            property.ApplyAttribute<VersionHistoryAttribute>(attribute => {
                deprecatedMajorVersion = attribute.DeprecatedMajorVersion;
                deprecatedMinorVersion = attribute.DeprecatedMinorVersion;
            });

            Assert.Equal(2, deprecatedMajorVersion);
            Assert.Equal(1, deprecatedMinorVersion);
        }

        [Fact]
        public void ApplyAttribute_GivenAttributeNotPresent_ThenDoesNotExecuteAction()
        {
            var property = typeof(TestClass).GetProperty("OldProperty");

            var isActionExecuted = false;

            property.ApplyAttribute<RequiredAttribute>(attribute => {
                isActionExecuted = true;
            });

            Assert.False(isActionExecuted);
        }

        [Fact]
        public void GetSerialisationName_GivenAttributePresent_ThenReturnsJsonPropertyName()
        {
            var property = typeof(TestClass).GetProperty("NewProperty");

            Assert.Equal("myNewName", property.GetSerialisationName());
        }

        [Fact]
        public void GetSerialisationName_GivenAttributeNotPresent_ThenReturnsPropertyNameAsCamelCase()
        {
            var property = typeof(TestClass).GetProperty("OldProperty");

            Assert.Equal("oldProperty", property.GetSerialisationName());
        }

        [Fact]
        public void GetVersionHistory_GivenAttributePresent_ThenReturnsVersionHistory()
        {
            var property = typeof(TestClass).GetProperty("OldProperty");

            var versionHistory = property.GetVersionHistory();

            Assert.NotNull(versionHistory?.Introduced);
            Assert.Equal(0, versionHistory.Introduced.Major);
            Assert.Equal(0, versionHistory.Introduced.Minor);

            Assert.NotNull(versionHistory?.Deprecated);
            Assert.Equal(2, versionHistory.Deprecated.Major);
            Assert.Equal(1, versionHistory.Deprecated.Minor);
        }

        [Fact]
        public void GetVersionHistory_GivenAttributeNotPresent_ThenReturnsDefault()
        {
            var property = typeof(TestClass).GetProperty("LongRunningProperty");

            var versionHistory = property.GetVersionHistory();

            Assert.NotNull(versionHistory?.Introduced);
            Assert.Equal(0, versionHistory.Introduced.Major);
            Assert.Equal(0, versionHistory.Introduced.Minor);

            Assert.NotNull(versionHistory?.Deprecated);
            Assert.Equal(0, versionHistory.Deprecated.Major);
            Assert.Equal(0, versionHistory.Deprecated.Minor);
        }

    }
}

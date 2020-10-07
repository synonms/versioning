using System;
using Xunit;

namespace Synonms.Versioning.Core.Tests
{
    public class VersionHistoryTests
    {
        [Fact]
        public void Constructor_GivenValues_ThenPopulatesProperties()
        {
            var versionHistory = new VersionHistory(new Version(1, 2), new Version(3, 4));

            Assert.NotNull(versionHistory?.Introduced);
            Assert.Equal(1, versionHistory.Introduced.Major);
            Assert.Equal(2, versionHistory.Introduced.Minor);

            Assert.NotNull(versionHistory?.Deprecated);
            Assert.Equal(3, versionHistory.Deprecated.Major);
            Assert.Equal(4, versionHistory.Deprecated.Minor);
        }

        [Fact]
        public void IsApplicableAtVersion_GivenDefault_ThenReturnsTrue()
        {
            var versionHistory = new VersionHistory(new Version());

            Assert.True(versionHistory.IsApplicableAtVersion(new Version(2, 0)));
        }

        [Fact]
        public void IsApplicableAtVersion_GivenIntroducedBefore_AndNotDeprecated_ThenReturnsTrue()
        {
            var versionHistory = new VersionHistory(new Version(1, 0));

            Assert.True(versionHistory.IsApplicableAtVersion(new Version(2, 0)));
        }

        [Fact]
        public void IsApplicableAtVersion_GivenIntroducedAfter_AndNotDeprecated_ThenReturnsFalse()
        {
            var versionHistory = new VersionHistory(new Version(2, 3));

            Assert.False(versionHistory.IsApplicableAtVersion(new Version(2, 0)));
        }

        [Fact]
        public void IsApplicableAtVersion_GivenIntroducedBefore_AndDeprecatedAfter_ThenReturnsTrue()
        {
            var versionHistory = new VersionHistory(new Version(1, 0), new Version(2, 3));

            Assert.True(versionHistory.IsApplicableAtVersion(new Version(2, 0)));
        }

        [Fact]
        public void IsApplicableAtVersion_GivenIntroducedBefore_AndDeprecatedBefore_ThenReturnsFalse()
        {
            var versionHistory = new VersionHistory(new Version(1, 0), new Version(1, 8));

            Assert.False(versionHistory.IsApplicableAtVersion(new Version(2, 0)));
        }

        [Fact]
        public void IsApplicableAtVersion_GivenIntroducedAfter_AndDeprecatedAfter_ThenReturnsFalse()
        {
            var versionHistory = new VersionHistory(new Version(2, 2), new Version(2, 6));

            Assert.False(versionHistory.IsApplicableAtVersion(new Version(2, 0)));
        }
    }
}

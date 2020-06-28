using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Synonms.Versioning.Core.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static void ApplyAttribute<TAttribute>(this PropertyInfo propertyInfo, Action<TAttribute> action)
        {
            if (propertyInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attribute)
            {
                action(attribute);
            }
        }

        public static string GetSerialisationName(this PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name.ToCamelCase();

            propertyInfo.ApplyAttribute<JsonPropertyNameAttribute>(attribute => name = attribute.Name);

            return name;
        }

        public static VersionHistory GetVersionHistory(this PropertyInfo propertyInfo)
        {
            var introducedVersion = new Version();
            Version deprecatedVersion = null;

            propertyInfo.ApplyAttribute<VersionHistoryAttribute>(attribute =>
            {
                introducedVersion = new Version(attribute.IntroducedMajorVersion, attribute.IntroducedMinorVersion);
                deprecatedVersion = new Version(attribute.DeprecatedMajorVersion, attribute.DeprecatedMinorVersion);
            });

            return new VersionHistory(introducedVersion, deprecatedVersion);
        }
    }
}

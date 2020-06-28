using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Synonms.Versioning.Core.Extensions
{
    public static class TypeExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        }

        public static void ApplyAttribute<TAttribute>(this Type type, Action<TAttribute> action) where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attribute)
            {
                action(attribute);
            }
        }

        public static VersionHistory GetVersionHistory(this Type type)
        {
            var introducedVersion = new Version();
            Version deprecatedVersion = null;

            type.ApplyAttribute<VersionHistoryAttribute>(attribute =>
            {
                introducedVersion = new Version(attribute.IntroducedMajorVersion, attribute.IntroducedMinorVersion);
                deprecatedVersion = new Version(attribute.DeprecatedMajorVersion, attribute.DeprecatedMinorVersion);
            });

            return new VersionHistory(introducedVersion, deprecatedVersion);
        }

        public static PropertyInfo GetPropertyBySerialisationName(this Type type, string name)
        {
            var propertyInfo = type.GetPropertiesWithAttribute<JsonPropertyNameAttribute>(attribute => attribute.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (propertyInfo == null)
            {
                propertyInfo = type.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            }

            return propertyInfo;
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<TAttribute>(this Type type, Func<TAttribute, bool> predicate) where TAttribute : Attribute
        {
            var output = new List<PropertyInfo>();

            foreach(var propertyInfo in type.GetProperties())
            {
                foreach(var attribute in propertyInfo.GetCustomAttributes(typeof(TAttribute), true))
                {
                    if (!(attribute is TAttribute attributeToCheck)) continue;

                    if (predicate(attributeToCheck))
                    {
                        output.Add(propertyInfo);
                    }
                }
            }

            return output;
        }

        public static string GetSerialisationName(this Type type)
        {
            var name = type.Name;

            type.ApplyAttribute<JsonPropertyNameAttribute>(attribute => name = attribute.Name);

            return name;
        }
    }
}

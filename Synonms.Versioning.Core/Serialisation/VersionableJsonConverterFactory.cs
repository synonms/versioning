using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Synonms.Versioning.Core.Serialisation
{
    public class VersionableJsonConverterFactory : JsonConverterFactory
    {
        private readonly Version _version;

        public VersionableJsonConverterFactory(Version version)
        {
            _version = version;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert == null) return false;
            if (typeToConvert.IsPrimitive) return false;
            if (typeToConvert.IsValueType) return false;
            if (typeToConvert == typeof(string)) return false;
            if (!typeToConvert.IsClass) return false;

            return true;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter)Activator.CreateInstance(
                typeof(VersionableJsonConverter<>).MakeGenericType(new Type[] { typeToConvert }),
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new object[] { _version },
                null);
        }
    }
}

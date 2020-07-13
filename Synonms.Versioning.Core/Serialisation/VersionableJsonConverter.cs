using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Synonms.Versioning.Core.Extensions;

namespace Synonms.Versioning.Core.Serialisation
{
    public class VersionableJsonConverter<TVersionable> : JsonConverter<TVersionable>
    {
        private readonly Version _version;

        public VersionableJsonConverter(Version version)
        {
            _version = version;
        }

        public override TVersionable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var output = Activator.CreateInstance<TVersionable>();

            var objectVersionHistory = typeof(TVersionable).GetVersionHistory();

            while(reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return output;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var propertyName = reader.GetString();
                var propertyInfo = typeof(TVersionable).GetPropertyBySerialisationName(propertyName);

                if (propertyInfo != null)
                {
                    var propertyVersionHistory = propertyInfo.GetVersionHistory();
                    var applicableVersionHistory = VersionHistory.Merge(propertyVersionHistory, objectVersionHistory);

                    if (applicableVersionHistory.IsApplicableAtVersion(_version))
                    {
                        var value = JsonSerializer.Deserialize(ref reader, propertyInfo.PropertyType, options);
                        propertyInfo.SetValue(output, value);
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, TVersionable value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            var objectVersionHistory = typeof(TVersionable).GetVersionHistory();

            foreach (var propertyInfo in typeof(TVersionable).GetProperties())
            {
                var propertyVersionHistory = propertyInfo.GetVersionHistory();
                var applicableVersionHistory = VersionHistory.Merge(propertyVersionHistory, objectVersionHistory);

                if (applicableVersionHistory.IsApplicableAtVersion(_version))
                {
                    writer.WritePropertyName(propertyInfo.GetSerialisationName());

                    JsonSerializer.Serialize(writer, propertyInfo.GetValue(value), options);
                }
            }

            writer.WriteEndObject();
        }
    }
}

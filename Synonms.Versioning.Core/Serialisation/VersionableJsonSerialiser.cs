using System;
using System.Text.Json;

namespace Synonms.Versioning.Core.Serialisation
{
    public class VersionableJsonSerialiser : IVersionableSerialiser
    {
        public T Deserialise<T>(string text, Version version)
        {
            var serialiserOptions = new JsonSerializerOptions
            {
                Converters = { new VersionableJsonConverterFactory(version) }
            };

            return JsonSerializer.Deserialize<T>(text, serialiserOptions);
        }

        public string Serialise<T>(T model, Version version)
        {
            var serialiserOptions = new JsonSerializerOptions
            {
                Converters = { new VersionableJsonConverterFactory(version) }
            };

            return JsonSerializer.Serialize(model, serialiserOptions);
        }
    }
}

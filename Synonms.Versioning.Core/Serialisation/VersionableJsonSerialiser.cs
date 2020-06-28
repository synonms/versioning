using System;
using System.Text.Json;

namespace Synonms.Versioning.Core.Serialisation
{
    public class VersionableJsonSerialiser<TVersionable> : IVersionableSerialiser<TVersionable> where TVersionable : IVersionable
    {
        public TVersionable Deserialise(string text, Version version)
        {
            var serialiserOptions = new JsonSerializerOptions
            {
                Converters = { new VersionableJsonConverter<TVersionable>(version) }
            };

            return JsonSerializer.Deserialize<TVersionable>(text, serialiserOptions);
        }

        public string Serialise(TVersionable model, Version version)
        {
            var serialiserOptions = new JsonSerializerOptions
            {
                Converters = { new VersionableJsonConverter<TVersionable>(version) }
            };

            return JsonSerializer.Serialize(model, serialiserOptions);
        }
    }
}

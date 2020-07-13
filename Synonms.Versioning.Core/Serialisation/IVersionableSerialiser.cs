using System;

namespace Synonms.Versioning.Core.Serialisation
{
    public interface IVersionableSerialiser
    {
        string Serialise<T>(T model, Version version);
        T Deserialise<T>(string text, Version version);
    }
}

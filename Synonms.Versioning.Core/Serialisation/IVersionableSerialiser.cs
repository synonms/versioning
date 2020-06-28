using System;

namespace Synonms.Versioning.Core.Serialisation
{
    public interface IVersionableSerialiser<TVersionable> where TVersionable : IVersionable
    {
        string Serialise(TVersionable model, Version version);
        TVersionable Deserialise(string text, Version version);
    }
}

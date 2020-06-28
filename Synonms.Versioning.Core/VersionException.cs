using System;
using System.Runtime.Serialization;

namespace Synonms.Versioning.Core
{
    [Serializable]
    public class VersionException : Exception
    {
        public VersionException() {}
        public VersionException(string message) : base(message) { }
        public VersionException(string message, Exception inner) : base(message, inner) { }
        protected VersionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

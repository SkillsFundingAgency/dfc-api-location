using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DFC.Api.Location.CutomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class DfcNullConfigValueException : Exception
    {
        public DfcNullConfigValueException()
        {
        }

        public DfcNullConfigValueException(string key)
        : base($"The config key {key} is null")
        {
        }

        public DfcNullConfigValueException(string key, Exception ex)
        : base($"The config key {key} is null", ex)
        {
        }

        protected DfcNullConfigValueException(SerializationInfo serializationInfo, StreamingContext streamingContext)
           : base(serializationInfo, streamingContext)
        {
        }
    }
}

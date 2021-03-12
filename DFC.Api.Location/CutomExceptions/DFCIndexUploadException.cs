using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DFC.Api.Location.CutomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class DfcIndexUploadException : Exception
    {
        public DfcIndexUploadException()
        {
        }

        public DfcIndexUploadException(string message)
        : base(message)
        {
        }

        public DfcIndexUploadException(string message, Exception ex)
        : base(message, ex)
        {
        }

        protected DfcIndexUploadException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}

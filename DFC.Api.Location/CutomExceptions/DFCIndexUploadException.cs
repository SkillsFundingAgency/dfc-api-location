using System;
using System.Diagnostics.CodeAnalysis;

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
    }
}

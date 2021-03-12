using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.Location.CutomExceptions
{
    [ExcludeFromCodeCoverage]
    public class DFCIndexUploadException : Exception
    {
        public DFCIndexUploadException()
        {
        }

        public DFCIndexUploadException(string message)
        : base(message)
        {
        }

        public DFCIndexUploadException(string message, Exception ex)
        : base(message, ex)
        {
        }
    }
}

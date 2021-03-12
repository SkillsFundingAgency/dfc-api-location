using System;
using System.Diagnostics.CodeAnalysis;

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
    }
}

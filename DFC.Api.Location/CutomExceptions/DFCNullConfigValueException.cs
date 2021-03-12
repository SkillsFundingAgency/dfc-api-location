using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.Location.CutomExceptions
{
    [ExcludeFromCodeCoverage]
    public class DFCNullConfigValueException : Exception
    {
        public DFCNullConfigValueException()
        {
        }

        public DFCNullConfigValueException(string key)
        : base($"The config key {key} is null")
        {
        }

        public DFCNullConfigValueException(string key, Exception ex)
        : base($"The config key {key} is null", ex)
        {
        }
    }
}

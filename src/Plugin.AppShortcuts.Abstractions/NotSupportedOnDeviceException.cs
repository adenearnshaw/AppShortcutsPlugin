using System;
namespace Plugin.AppShortcuts.Abstractions
{
    public class NotSupportedOnDeviceException : Exception
    {
        public NotSupportedOnDeviceException() : base()
        { }

        public NotSupportedOnDeviceException(string message) : base(message)
        { }

        public NotSupportedOnDeviceException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}

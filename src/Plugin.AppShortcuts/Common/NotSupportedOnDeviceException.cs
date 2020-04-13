using System;

namespace Plugin.AppShortcuts
{
    /// <summary>
    /// Feature is not supported by target device's API
    /// </summary>
    public class NotSupportedOnDeviceException : Exception
    {
        /// <summary>
        /// Feature is not supported by target device's API
        /// </summary>
        public NotSupportedOnDeviceException() : base()
        { }

        /// <summary>
        /// Feature is not supported by target device's API
        /// </summary>
        public NotSupportedOnDeviceException(string message) : base(message)
        { }

        /// <summary>
        /// Feature is not supported by target device's API
        /// </summary>
        public NotSupportedOnDeviceException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}

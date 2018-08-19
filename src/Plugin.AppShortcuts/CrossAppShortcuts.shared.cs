using System;

namespace Plugin.AppShortcuts
{
    /// <summary>
    /// Cross Platform App Shortcuts
    /// </summary>
    public static class CrossAppShortcuts
    {
        static Lazy<IAppShortcuts> implementation
            = new Lazy<IAppShortcuts>(() => CreateAppShortcuts(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => (implementation?.Value as IPlatformSupport)?.IsSupportedByCurrentPlatformVersion ?? false;

        /// <summary>
        /// Current plugin implementation to use
        /// </summary>
        public static IAppShortcuts Current
        {
            get
            {
                IAppShortcuts ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IAppShortcuts CreateAppShortcuts()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
            return new AppShortcutsImplementation();
#pragma warning restore IDE0022 // Use expression body for methods
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly. You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
}

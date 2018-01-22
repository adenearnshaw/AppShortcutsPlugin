using System;
using Plugin.AppShortcuts.Abstractions;

namespace Plugin.AppShortcuts
{
    public class CrossAppShortcuts
    {
        static Lazy<IAppShortcuts> implementation
            = new Lazy<IAppShortcuts>(() => CreateAppShortcuts(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static bool IsSupported => (implementation?.Value as IPlatformSupport)?.IsSupportedByCurrentPlatformVersion ?? false;

        public static IAppShortcuts Current
        {
            get
            {
                var ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IAppShortcuts CreateAppShortcuts()
        {
#if NETSTANDARD1_0
            return null;
#else
            return new AppShortcutsImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly. You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
}

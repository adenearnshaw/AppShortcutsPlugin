namespace Plugin.AppShortcuts
{
    internal interface IPlatformSupport
    {
        bool IsSupportedByCurrentPlatformVersion { get; }
    }
}

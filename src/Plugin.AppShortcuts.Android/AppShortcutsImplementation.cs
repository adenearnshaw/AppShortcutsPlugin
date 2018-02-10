using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.AppShortcuts.Abstractions;
using AUri = Android.Net.Uri;

namespace Plugin.AppShortcuts
{
    public class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
    {
        private readonly ShortcutManager _manager;
        private readonly bool _isShortcutsSupported;

        public AppShortcutsImplementation()
        {
            _manager = (ShortcutManager)Application.Context.GetSystemService(Context.ShortcutService);

            _isShortcutsSupported = Build.VERSION.SdkInt >= BuildVersionCodes.N;
        }

        public bool IsSupportedByCurrentPlatformVersion => _isShortcutsSupported;

        public Task AddShortcut(Shortcut shortcut)
        {
            return Task.Run(() =>
            {
                if (!_isShortcutsSupported)
                    return;

                var context = Application.Context;
                var builder = new ShortcutInfo.Builder(context, shortcut.ID);

                var uri = AUri.Parse(shortcut.Uri);

                builder.SetIntent(new Intent(Intent.ActionView, uri));
                builder.SetShortLabel(shortcut.Label);
                builder.SetLongLabel(shortcut.Description);

                var scut = builder.Build();

                if (_manager.DynamicShortcuts == null || !_manager.DynamicShortcuts.Any())
                    _manager.SetDynamicShortcuts(new List<ShortcutInfo> { scut });
                else
                    _manager.AddDynamicShortcuts(new List<ShortcutInfo> { scut });
            });
        }

        public Task<List<Shortcut>> GetShortcuts()
        {
            return Task.Run(() =>
            {
                if (!_isShortcutsSupported)
                    return new List<Shortcut>();

                var dynamicShortcuts = _manager.DynamicShortcuts;
                var shortcuts = dynamicShortcuts.Select(s => new Shortcut(s.Id)
                {
                    Label = s.ShortLabel,
                    Description = s.LongLabel,
                    Uri = s.Intent.ToUri(IntentUriType.AllowUnsafe)
                }).ToList();
                return shortcuts;
            });
        }

        public Task RemoveShortcut(string shortcutId)
        {
            return Task.Run(() =>
            {
                if (!_isShortcutsSupported)
                    return;

                _manager.RemoveDynamicShortcuts(new List<string> { shortcutId });
            });
        }
    }
}

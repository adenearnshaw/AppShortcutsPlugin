using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.StartScreen;
using Plugin.AppShortcuts.Icons;
using Plugin.AppShortcuts.UWP;

namespace Plugin.AppShortcuts
{
    internal class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
    {
        private readonly IIconProvider _embeddedIconProvider;
        private readonly IIconProvider _customIconProvider;

        public AppShortcutsImplementation()
        {
            IsSupportedByCurrentPlatformVersion = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2) && JumpList.IsSupported();

            _embeddedIconProvider = new EmbeddedIconProvider();
            _customIconProvider = new CustomIconProvider();
        }

        public void Init()
        {
        }

        public bool IsSupportedByCurrentPlatformVersion { get; }

        public async Task AddShortcut(Shortcut shortcut)
        {
            var args = JumplistArgumentsHelper.GetSerializedArguments(shortcut.ShortcutId, shortcut.Uri, shortcut.Tag);
            var jumplistItem = JumpListItem.CreateWithArguments(args, shortcut.Label);
            jumplistItem.Description = shortcut.Description;
            jumplistItem.Logo = await GetIconUri(shortcut.Icon);

            var jumplist = await JumpList.LoadCurrentAsync();
            jumplist.Items.Add(jumplistItem);

            await jumplist.SaveAsync();
        }

        public async Task<List<Shortcut>> GetShortcuts()
        {
            var jumplist = await JumpList.LoadCurrentAsync();

            var shortcuts = jumplist.Items.Select(i =>
            {
                var args = JumplistArgumentsHelper.DeserializeArguments(i.Arguments);
                var sc = new Shortcut(args.ShortcutId)
                {
                    Label = i.DisplayName,
                    Description = i.Description,
                    Icon = new DefaultIcon(),
                    Uri = args.Uri,
                    Tag = args.Tag
                };
                return sc;
            }).ToList();

            return shortcuts;
        }

        public async Task RemoveShortcut(string shortcutId)
        {
            var jumplist = await JumpList.LoadCurrentAsync();
            var toDelete = jumplist.Items.FirstOrDefault(i => i.Arguments.Contains(shortcutId));

            if (toDelete == null)
                return;

            jumplist.Items.Remove(toDelete);
            await jumplist.SaveAsync();
        }

        private async Task<Uri> GetIconUri(IShortcutIcon shortcutIcon)
        {
            if (shortcutIcon is CustomIcon)
                return await _customIconProvider.CreatePlatformIcon(shortcutIcon) as Uri;

            return await _embeddedIconProvider.CreatePlatformIcon(shortcutIcon) as Uri;
        }
    }
}

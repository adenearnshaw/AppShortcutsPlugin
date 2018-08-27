using System.Threading.Tasks;
using Plugin.AppShortcuts.Icons;

namespace Plugin.AppShortcuts.UWP
{
    internal class EmbeddedIconProvider : IIconProvider
    {
        private const string DarkIconUriFormat = "icon_plugin_{0}_white.png";

        private readonly EmbeddedImageHelper _embeddedImageHelper;

        public EmbeddedIconProvider()
        {
            _embeddedImageHelper = new EmbeddedImageHelper();
        }

        public async Task<object> CreatePlatformIcon(IShortcutIcon shortcutIcon)
        {
            var iconFileName = string.Format(DarkIconUriFormat, shortcutIcon.IconName.ToLower());

            var uri = await _embeddedImageHelper.CopyEmbeddedImageToAppData(iconFileName);
            return uri;
        }
    }
}

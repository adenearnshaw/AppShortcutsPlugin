using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Plugin.AppShortcuts.iOS;
using Plugin.AppShortcuts.Icons;
using UIKit;

namespace Plugin.AppShortcuts
{
    [Preserve(AllMembers = true)]
    internal partial class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
    {
        private const string SHORTCUT_URI_KEY = "ShortcutUri";
        private readonly string NOT_SUPPORTED_ERROR_MESSAGE 
            = $"Operation not supported on iOS 8 or below. Use {nameof(CrossAppShortcuts)}.{nameof(CrossAppShortcuts.IsSupported)} to check if the current device supports this feature.";

        private readonly IIconProvider _embeddedIconProvider;
        private readonly IIconProvider _customIconProvider;
        private readonly bool _isShortcutsSupported;

        public AppShortcutsImplementation()
        {
            _embeddedIconProvider = new EmbeddedIconProvider();
            _customIconProvider = new CustomIconProvider();
            _isShortcutsSupported = UIDevice.CurrentDevice.CheckSystemVersion(9, 0);
        }

        public void Init()
        { }

        public bool IsSupportedByCurrentPlatformVersion => _isShortcutsSupported;

        public async Task AddShortcut(Shortcut shortcut)
        {
            if (!_isShortcutsSupported)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            var type = shortcut.ShortcutId;
            var icon = shortcut.IsEmbeddedIcon
                ? (UIApplicationShortcutIcon) (await _embeddedIconProvider.CreatePlatformIcon(shortcut.Icon))
                : (UIApplicationShortcutIcon) (await _customIconProvider.CreatePlatformIcon(shortcut.Icon));
            var metadata = CreateUriMetadata(shortcut.Uri);

            new NSObject().BeginInvokeOnMainThread(() =>
            {
                var scut = new UIMutableApplicationShortcutItem(type,
                    shortcut.Label,
                    shortcut.Description,
                    icon,
                    metadata);

                var scuts = UIApplication.SharedApplication.ShortcutItems.ToList();
                scuts.Add(scut);

                UIApplication.SharedApplication.ShortcutItems = scuts.ToArray();
            });
        }

        public async Task<List<Shortcut>> GetShortcuts()
        {
            if (!_isShortcutsSupported)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            var dynamicShortcuts = UIApplication.SharedApplication.ShortcutItems.ToList();
            var shortcuts = dynamicShortcuts.Select(ds => new Shortcut(ds.Type)
            {
                Label = ds.LocalizedTitle,
                Description = ds.LocalizedSubtitle,
                Uri = ds?.UserInfo[SHORTCUT_URI_KEY]?.ToString() ?? string.Empty,
                Icon = ResolveShortcutIconType(ds?.Icon?.ToString() ?? string.Empty)
            }).ToList();

            await Task.Delay(200);

            return shortcuts;
        }

        public async Task RemoveShortcut(string shortcutId)
        {
            if (!_isShortcutsSupported)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            new NSObject().BeginInvokeOnMainThread(() =>
            {
                var shortcutItem =
                    UIApplication.SharedApplication.ShortcutItems.FirstOrDefault(si => si.Type.Equals(shortcutId));

                if (shortcutItem == null)
                    return;

                var updatedItems = UIApplication.SharedApplication.ShortcutItems.ToList();
                updatedItems.Remove(shortcutItem);
                UIApplication.SharedApplication.ShortcutItems = updatedItems.ToArray();
            });
        }

        private NSDictionary<NSString, NSObject> CreateUriMetadata(string uri)
        {
            var metadata = new NSDictionary<NSString, NSObject>(new NSString(SHORTCUT_URI_KEY), new NSString(uri));
            return metadata;
        }
        
        private static Func<string, IShortcutIcon> ResolveShortcutIconType = iconName =>
        {
            var isParseSuccessful = Enum.TryParse(iconName, out ShortcutIconType type);

            if (isParseSuccessful)
                return ResolveEmbeddedIcon(type);

            return new CustomIcon(iconName);
        };

        private static Func<ShortcutIconType, EmbeddedIcon> ResolveEmbeddedIcon = iconType =>
        {
            switch (iconType)
            {
                case ShortcutIconType.Add:
                    return new AddIcon();
                case ShortcutIconType.Alarm:
                    return new AlarmIcon();
                case ShortcutIconType.Audio:
                    return new AudioIcon();
                case ShortcutIconType.Bookmark:
                    return new BookmarkIcon();
                case ShortcutIconType.CapturePhoto:
                    return new CapturePhotoIcon();
                case ShortcutIconType.CaptureVideo:
                    return new CaptureVideoIcon();
                case ShortcutIconType.Cloud:
                    return new CloudIcon();
                case ShortcutIconType.Compose:
                    return new ComposeIcon();
                case ShortcutIconType.Confirmation:
                    return new ConfirmationIcon();
                case ShortcutIconType.Contact:
                    return new ContactIcon();
                case ShortcutIconType.Date:
                    return new DateIcon();
                case ShortcutIconType.Favorite:
                    return new FavoriteIcon();
                case ShortcutIconType.Home:
                    return new HomeIcon();
                case ShortcutIconType.Invitation:
                    return new InvitationIcon();
                case ShortcutIconType.Location:
                    return new LocationIcon();
                case ShortcutIconType.Love:
                    return new LoveIcon();
                case ShortcutIconType.Mail:
                    return new MailIcon();
                case ShortcutIconType.MarkLocation:
                    return new MarkLocationIcon();
                case ShortcutIconType.Message:
                    return new MessageIcon();
                case ShortcutIconType.Pause:
                    return new PauseIcon();
                case ShortcutIconType.Play:
                    return new PlayIcon();
                case ShortcutIconType.Prohibit:
                    return new ProhibitIcon();
                case ShortcutIconType.Search:
                    return new SearchIcon();
                case ShortcutIconType.Share:
                    return new ShareIcon();
                case ShortcutIconType.Shuffle:
                    return new ShuffleIcon();
                case ShortcutIconType.Task:
                    return new TaskIcon();
                case ShortcutIconType.TaskCompleted:
                    return new TaskCompletedIcon();
                case ShortcutIconType.Time:
                    return new TimeIcon();
                case ShortcutIconType.Update:
                    return new UpdateIcon();
                case ShortcutIconType.Default:
                default:
                    return new DefaultIcon();
            }
        };
    }
}

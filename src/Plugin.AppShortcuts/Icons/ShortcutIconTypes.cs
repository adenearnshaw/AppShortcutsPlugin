namespace Plugin.AppShortcuts.Icons
{
    internal enum ShortcutIconType
    {
        Default       = 1,
        Add           = 2,
        Alarm         = 3,
        Audio         = 4,
        Bookmark      = 5,
        CapturePhoto  = 6,
        CaptureVideo  = 7,
        Cloud         = 8,
        Compose       = 9,
        Confirmation  = 10,
        Contact       = 11,
        Date          = 12,
        Favorite      = 13,
        Home          = 14,
        Invitation    = 15,
        Location      = 16,
        Love          = 17,
        Mail          = 18,
        MarkLocation  = 19,
        Message       = 20,
        Pause         = 21,
        Play          = 22,
        Prohibit      = 23,
        Search        = 24,
        Share         = 25,
        Shuffle       = 26,
        Task          = 27,
        TaskCompleted = 28,
        Time          = 29,
        Update        = 30
    }

    internal static class ShortcutIconTypesHelper
    {
        internal static EmbeddedIcon ResolveEmbeddedIcon(ShortcutIconType iconType)
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
        }
    }
}

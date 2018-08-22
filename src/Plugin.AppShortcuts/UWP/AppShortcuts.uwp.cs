using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.StartScreen;
using Plugin.AppShortcuts.Icons;
using Plugin.AppShortcuts.UWP;
using System.Runtime.Serialization.Json;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace Plugin.AppShortcuts
{
    public class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
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
            var args = GetSerializedArguments(shortcut.ShortcutId, shortcut.Uri);
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
                var args = DeserializeArguments(i.Arguments);
                var sc = new Shortcut(args.ShortcutId)
                {
                    Label = i.DisplayName,
                    Description = i.Description,
                    Icon = new DefaultIcon(),
                    Uri = args.Uri
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

        private string GetSerializedArguments(string shortcutId, string uri)
        {
            var args = new JumpListArguments
            {
                ShortcutId = shortcutId,
                Uri = uri
            };

            using (var ms = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(typeof(JumpListArguments));
                ser.WriteObject(ms, args);
                byte[] json = ms.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }

        private JumpListArguments DeserializeArguments(string json)
        {
            // Support for backwards compatibility from v0.5
            if (json.Contains("||"))
            {
                var parts = json.Split(new[] {"||"}, StringSplitOptions.RemoveEmptyEntries);
                return new JumpListArguments(parts[0], parts[1]);
            }

            var args = new JumpListArguments();
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var ser = new DataContractJsonSerializer(args.GetType());
                args = ser.ReadObject(ms) as JumpListArguments;
                return args;
            }
        }

        [DataContract]
        class JumpListArguments
        {
            [DataMember]
            public string ShortcutId { get; set; }
            [DataMember]
            public string Uri { get; set; }

            public JumpListArguments()
            { }

            public JumpListArguments(string shortcutId, string uri)
            {
                ShortcutId = shortcutId;
                Uri = uri;
            }
        }
    }
}
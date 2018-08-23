using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Plugin.AppShortcuts.UWP
{
    /// <summary>
    /// Helper to abstract url from jumplist arguments
    /// </summary>
    public static class JumplistArgumentsHelper
    {
        /// <summary>
        /// Returns uri from Jumplist Arguments
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns>Uri parsed from Arguments</returns>
        public static Uri GetUriFromJumplistArguments(string arguments)
        {
            if (string.IsNullOrEmpty(arguments))
                return new Uri("");

            var args = DeserializeArguments(arguments);
            return new Uri(args.Uri);
        }

        internal static string GetSerializedArguments(string shortcutId, string uri)
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

        internal static JumpListArguments DeserializeArguments(string json)
        {
            // Support for backwards compatibility from v0.5
            if (json.Contains("||"))
            {
                var parts = json.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
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
        internal class JumpListArguments
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
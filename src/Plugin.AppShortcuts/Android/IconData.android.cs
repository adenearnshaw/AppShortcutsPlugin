using System;
using System.IO;

namespace Plugin.AppShortcuts.Android
{
    internal abstract class IconData
    {
        internal abstract string Name { get; }
        internal abstract string SvgData { get; }

        public string GetSvgPath()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fileName = $"{Name}.xml";
            var filePath = Path.Combine(folderPath, fileName);

            if (!File.Exists(filePath))
            {
                CreateFile(filePath);
            }

            return filePath;
        }

        private void CreateFile(string filePath)
        {
            File.Delete(filePath);
            File.WriteAllText(filePath, SvgData);
        }
    }
}

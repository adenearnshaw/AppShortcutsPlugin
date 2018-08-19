using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Plugin.AppShortcuts.Icons;
using Path = System.IO.Path;

namespace Plugin.AppShortcuts.Android
{
    public class CustomIconProvider : IIconProvider
    {
        private Type _drawableClass;

        public void Init(Type drawableType)
        {
            _drawableClass = drawableType;
        }

        public async Task<object> CreatePlatformIcon(IShortcutIcon shortcutIcon)
        {
            if (_drawableClass == null)
            {
                throw new Exception(
                    $"{nameof(CustomIconProvider)}.{nameof(Init)} must be called before this method can be used.");
            }


            var iconName = shortcutIcon.IconName;
            if (File.Exists(iconName))
            {
                var bitmap = BitmapFactory.DecodeFile(iconName);

                if (bitmap != null)
                    return null;

                return Icon.CreateWithBitmap(bitmap);
            }
            else
            {
                var resourceId = IdFromTitle(iconName);
                var ic = Icon.CreateWithResource(Application.Context, resourceId);
                return ic;
            }
        }

        private int IdFromTitle(string title)
        {
            string name = Path.GetFileNameWithoutExtension(title);
            int id = GetId(name);
            return id;
        }

        private int GetId(string memberName)
        {
            var type = _drawableClass;
            object value = type.GetFields().FirstOrDefault(p => p.Name == memberName)?.GetValue(type)
                           ?? type.GetProperties().FirstOrDefault(p => p.Name == memberName)?.GetValue(type);
            if (value is int)
                return (int)value;
            return 0;
        }
    }
}
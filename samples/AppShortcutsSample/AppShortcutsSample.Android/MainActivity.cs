using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.AppShortcuts;

namespace AppShortcutsSample.Droid
{
    [Activity(Label = "AppShortcuts Sample",
              Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionView },
                  Categories = new[] { Intent.CategoryDefault },
                  DataScheme = "stc",
                  DataHost = "appshortcuts",
                  AutoVerify = true)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, bundle);
            CrossAppShortcuts.Current.Init();

            LoadApplication(new AppShortcutsSample.App());
        }
    }
}


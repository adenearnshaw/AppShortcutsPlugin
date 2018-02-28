using Android.App;
using Android.Content;
using Android.Support.V7.App;
using AppShortcutsSample.Droid;

[Activity(Label = "AppShortcuts Sample",
          Icon = "@drawable/AppIcon",
          Theme = "@style/SplashTheme",
          MainLauncher = true,
          NoHistory = true)]
public class SplashActivity : AppCompatActivity
{
    protected override void OnResume()
    {
        base.OnResume();
        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    }
}
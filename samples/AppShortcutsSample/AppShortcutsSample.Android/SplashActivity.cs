using Android.App;
using Android.Content;
using Android.Support.V7.App;

namespace AppShortcutsSample.Droid
{

    [Activity(Label = "AppShortcuts Sample",
              Icon = "@drawable/app_icon",
              RoundIcon = "@drawable/app_icon_round",
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
}
# Handling Deeplinks

To provide a consistent pattern when the user clicks a shortcut to launch the app, the code to handle the shortcut's uri, should be done in the `App` class' `OnAppLinkReceived()` method. In order for each platform to pass the uri to this method, some additional work is required.

## Android

In order for the platform to launch your app from a shortcut, it needs to be told that the shortcut's uri can be handled by the app. This is done by adding an `IntentFilter` attribute to the `MainActivity`.  

```csharp
[Activity(Label = "AppShortcuts Sample",
          Theme = "@style/MainTheme")]
[IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryDefault },
              DataScheme = "asc",
              DataHost = "AppShortcutsApp",
              AutoVerify = true)]
public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
{
    ...
```

This is all that needs to be done for Android, the `FormsAppCompatActivity` will look to see if the app launched with a Uri and pass this through to the `App` class.

* [Configuring Intent Filters](https://developer.xamarin.com/guides/android/platform_features/app-linking/#configure-intent-filter)


## iOS

iOS will automatically launch the app from a shortcut, however, the `FormsApplicationDelegate` is not setup to pass the data through to the `App` class. To do this, the AppDelegate's `PerformActionForShortcutItem()` needs to be overridden.

```csharp
public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
{
    if (shortcutItem.UserInfo.ContainsKey(new NSString("ShortcutUri")))
    {
        var shortcutUri = shortcutItem.UserInfo["ShortcutUri"].ToString();
        Xamarin.Forms.Application.Current.SendOnAppLinkRequestReceived(new Uri(shortcutUri));
    }
}
``` 


## UWP

Similar to iOS, a UWP app will launch the app when the shortcut is clicked, but needs some additional setup to pass the Uri back to the Forms `App` class.

A restriction of UWP is that its implementation of shortcuts doesn't provide the ability to set multiple properties. In order for The Plugin to overcome this, the shortcut's ID and Uri are concatenated by a double pipe.

Firstly, in the UWP `MainPage` add a method to split the NavigationArgs and pass this to the Forms `App`. Then override the `OnNavigatedTo()` to call it. Your UWP `MainPage` should look similar to this:

```csharp
public sealed partial class MainPage : WindowsPage
{
    public MainPage()
    {
        this.InitializeComponent();
        LoadApplication(new App());
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        OnLaunchedEvent(e.Parameter.ToString());
    }

        public void OnLaunchedEvent(string arguments)
        {
            if (!string.IsNullOrEmpty(arguments))
            {
                var parts = arguments.Split("||", StringSplitOptions.RemoveEmptyEntries);
                Xamarin.Forms.Application.Current.SendOnAppLinkRequestReceived(new Uri(parts[1]));
            }
        }
    }
```

Next, in the UWP `App` class, modify the `OnLaunched()` method so that if `rootFrame.Content` is not null, then it calls the MainPage's `OnLaunchedEvent()` directly:

```csharp
protected override void OnLaunched(LaunchActivatedEventArgs e)
{
    ...

    if (rootFrame.Content == null)
    {
        rootFrame.Navigate(typeof(MainPage), e.Arguments);
    }
    else
    {
        var page = rootFrame.Content as MainPage;
        page?.OnLaunchedEvent(e.Arguments);
    }
    Window.Current.Activate();
}
```

---

**To see this in the context of an app, please see the [sample](../samples) provided**

---
<= Back to [Table of Contents](README.md)
# Advanced scenarios

## Hooking into navigation for Master Detail pages

If your MainPage uses a MasterDetail page rather than a standard NavigationPage, you'll need to utilise the Navigation property within the MasterDetail.Detail object.

```csharp
await (MainPage as MasterDetailPage)?.Detail.Navigation.PushAsync(new ItemDetailPage());
```

A simple example of this can be found within the [AppShortcutsPluginSamples](https://github.com/adenearnshaw/AppShortcutsPluginSamples) repo.

## Opening links in the same Android instance each time

If the app is backgrounded and a shortcut is tapped, your app may relaunch and re-initialized the App from scratch, wiping any existing navigation backstack. This is caused by the MainActivity recreating itself when coming in via the IntentFilter. To re-launch the app in the same instance that was backgrounded add the following property to the `Activity` attribute in your **MainActivity**

```csharp
LaunchMode = LaunchMode.SingleTask
```

Thanks to [KerosenoDev](https://github.com/KerosenoDev) for raising [this](https://github.com/adenearnshaw/AppShortcutsPlugin/issues/28)


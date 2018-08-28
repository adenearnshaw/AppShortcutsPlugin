# Getting Started

## Setup
* Nuget: [Plugin.AppShortcuts](http://www.nuget.org/packages/Plugin.AppShortcuts)
* `PM> Install-Package Plugin.AppShortcuts`
* Install into the project where you'll be implementing the code and also each platform specific project.
* Namespace: `using Plugin.AppShortcuts;`

#### Android specific setup
For Android apps, the plugin needs some additional setup. In the `MainActivity.OnCreate()` before the `LoadApplication()` call, make a call to:  

`CrossAppShortcuts.Current.Init();`


## Plugin API

### Adding a shortcut

To add a shortcut, first you need to create a new instance of the `Shortcut` class and populate it with the information you wish to pin.

```csharp
using Plugin.AppShortcuts;
using Plugin.AppShortcuts.Icons;

...

var shortcut = new Shortcut()
{
    Label = "Shortcut 1",
    Description = "My shortcut",
    Icon = new FavoriteIcon(),
    Uri = "asc://AppShortcutsApp/Detail/shortcut_1_id"
};
```

There are a number of supplied icons that you can use, or you can also include a custom icon. Details of how to specify a custom icon can be found [here](CustomIcons.md).

Once you have your shorcut object, to add it to the App Icons shortcuts you call:

```csharp
await CrossAppShortcuts.Current.AddShortcut(shortcut);
```

*Note; you can add as any shortcuts as you wish, but only four will ever be displayed on the homepage. This is not tracked by the plugin.*

### Discovering shortcuts
The plugin provides a quick way of providing a list of all the current shortcuts.

```csharp
await CrossAppShortcuts.Current.GetShortcuts();
```

### Removing shortcuts
Each shortcut object is assigned an ID. To remove any given shortcut, call the `RemoveShortcut()` method with the given shortcut's ID.

```csharp
await CrossAppShortcuts.Current.RemoveShortcut("shortcut_1_id");
``` 

### Checking compatibility
App shortcuts are not supported on Android API level 24 or lower, or on iOS 8 and below. If you try and call this api on an unsupported device, a `NotSupportedOnDeviceException`. 

You can check if app shortcuts are supported on the current device using:

```csharp
CrossAppShortcuts.IsSupported
```

### Responding when the app is opened from a shortcut

Each platform handles app shortcuts differently, click [here](Deeplinks.md) for more information.

---

**To see this in the context of an app, please see the [sample](https://github.com/adenearnshaw/AppShortcutsPlugin/tree/master/samples) provided**

---

<= Back to [Table of Contents](README.md)
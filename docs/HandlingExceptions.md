# Handling Exceptions

## NotImplementedException

If you receive this error, then the Plugin has not been added as a nuget to the platform that you're currently targeting.

## NotSupportedOnDeviceException
App shortcuts are not supported on Android API level 24 or lower, or on iOS 8 and below. If you try and call this api on an unsupported device, this error is raised. 

You can check if app shortcuts are supported on the current device using:

```csharp
CrossAppShortcuts.IsSupported
```

---

**To see this in the context of an app, please see the [sample](https://github.com/adenearnshaw/AppShortcutsPlugin/tree/master/samples) provided**

---
<= Back to [Table of Contents](README.md)
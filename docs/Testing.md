# Testing App Shortcuts

While Android exposes AppShorcuts through a long press of the App's icon, iOS uses 3D Touch. Testing this can therefore sometime be a little tricky.

## Using iOS Simulator

The built in iOS Simulator does have the ability to test 3D Touch, providing you have a force-touch enabled trackpad and use the Simulator directly on the Mac (i.e. don't use VS Remote Simulator). Alternatively, there is an option under **Hardware>Touch Pressure>Deep Press** which will allow you to simulate 3D touch.

If you are using the Remote iOS Simulator provided by Visual Studio, then unfortunately, there's no way to currently test Force Touch. Although the shell does expose an option for Touch Mode, the Visual Studio team, as of yet, have not added Deep Press as an option you can select.

> Despite the Apple Magic Trackpad having force touch, you **cannot** use this with Windows as a work around.
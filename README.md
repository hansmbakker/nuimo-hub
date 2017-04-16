# Nuimo Hub
This is a hub application for a [Nuimo controller](https://www.senic.com/) running as a Windows UWP background task, for example on Raspberry PI and Windows 10 IoT Core.

The goal is to create an hub application that can run on an embedded device such as a Raspberry PI, so that Nuimo does not depend on your phone to function.

I started working on three integrations:
- Philips Hue
- Chromecast
- A kitchen timer

## Application status
The application does not work properly yet; this is a quick first commit so that other people can start looking at it and so that I have a place to commit my code to.

The main issue is that the application freezes after some time or when an integration handled around ~8 (rotation) events. I tried:
- enabling throwing on all Exceptions
- lowering the trigger interval by creating a throttled gesture event in the NuimoSDK (getsenic/nuimo-windows#16)
- unloading integrations to see whether it was caused by a certain piece of code

Other than that, the connecting logic and the device watching logic might need to be improved (potentially in the Nuimo SDK instead of this application)

## Integrations status
For most integrations there is groundwork but they are not finished yet.

### Philips Hue
- Uses Q42.HueApi
- Currently the connection settings are hardcoded in `HueApp.cs` until the Hue link button functionality is implemented.
- Oddly enough the application freezes when `await HueClient.GetGroupsAsync();` is called
- Code is mostly there but does not function well yet

### Chromecast
- Uses SharpCaster
- Implementation not finished yet

### Kitchen timer
- Time can be set by rotating the ring
- Displays the time on the LED matrix

### Test app
- Used to fiddle around with events and led matrices

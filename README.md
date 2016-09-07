# Nuimo Hub
This is a hub application for a [Nuimo controller](https://www.senic.com/) running as a Windows UWP background task, for example on Raspberry PI and Windows 10 IoT Core.

The goal is to create an hub application that can run on an embedded device such as a Raspberry PI, so that Nuimo does not depend on your phone to function.

I started working on three integrations:
- Philips Hue (settings in hueSettings.json are needed the Hue link button functionality is implemented)
- Chromecast
- A kitchen timer

The application does not work properly yet; this is a quick first commit so that other people can start looking at it and so that I have a place to commit my code to.

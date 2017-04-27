# Nuimo Hub
This is a hub application for a [Nuimo controller](https://www.senic.com/) running as a Windows UWP background task, for example on Raspberry PI and Windows 10 IoT Core.

The goal is to create an hub application that can run on an embedded device such as a Raspberry PI, so that Nuimo does not depend on your phone to function.

I started working on three integrations:
- Philips Hue
- Chromecast
- A kitchen timer

## Application status
Currently the hub is in a basic state.

Todo:

- [ ] connection stability
- [ ] device watching logic #1
- [ ] reconnection when the connection
- [ ] management interface
- [ ] any changes to make the hub application more robust.

## Integrations status
For most integrations there is groundwork but they are not finished yet.

### Philips Hue
- Uses Q42.HueApi
- Currently the connection settings are hardcoded in `HueApp.cs`
- Has support for three room types defined in the Philips Hue app (LivingRoom, Hallway and Bedroom)
- Swiping left/right allows you to select a room
- Button press switches light on/off
- Rotate changes brightness

Todo:

- [ ] implement Hue discovery #3
- [ ] implement the Hue link button functionality #2
- [ ] fix brightness adjustment / display
- [ ] make group / room management more reliable
  - [ ] groups are in room 'Other' by default
  - [ ] multiple groups in same room?

### Chromecast
- Uses SharpCaster
- Implementation not finished yet

### Kitchen timer
Basic working version:
- Time can be set by rotating the ring
- Displays the time on the LED matrix
- Button press starts countdown
- Second button press stops the countdown and resets the time
- Timer icon is flashed 10 times when countdown finishes

Todo:

- [ ] Do not allow to start countdown if TimeLeft is zero
- [ ] Make time display better
- [ ] Make time adjustment better

### Test app
- Used to fiddle around with events and led matrices

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuimoHelpers.LedMatrices;
using NuimoHub.Interfaces;
using NuimoSDK;
using Q42.HueApi;
using Q42.HueApi.Models.Groups;

namespace HueApp
{
    public class HueApp : INuimoApp
    {
        public HueApp()
        {
            Task.Run(async () => await SetupHue());
        }

        private HueClient HueClient { get; set; }

        private Dictionary<RoomClass, Group> GroupDictionary { get; set; }

        private LinkedList<RoomClass> Rooms { get; set; }
        private LinkedListNode<RoomClass> CurrentRoom { get; set; }

        private Dictionary<Group, bool> OnOffStateForGroups { get; set; }
        public string Name => "Hue";

        public NuimoLedMatrix Icon => Icons.LightBulb;

        public void OnFocus(INuimoHub nuimoHub)
        {
            //throw new NotImplementedException();
        }

        public void OnLostFocus(INuimoHub sender)
        {
            //throw new NotImplementedException();
        }

        public void OnGestureEvent(INuimoController controller, NuimoGestureEvent nuimoGestureEvent)
        {
            //var groupInControl = GroupForController[controller.Identifier];
            //IEnumerable<string> lights = groupInControl.Lights;

            switch (nuimoGestureEvent.Gesture)
            {
                case NuimoGesture.ButtonPress:
                    var newState = SwitchGroupOnOff();
                    var matrix = GetPowerMatrix(newState);
                    controller.DisplayLedMatrixAsync(matrix);
                    break;
                case NuimoGesture.ButtonRelease:
                    break;
                case NuimoGesture.Rotate:
                    ChangeBrightness(nuimoGestureEvent.Value);
                    //controller.DisplayLedMatrixAsync(NuimoLedMatrix)
                    break;
                case NuimoGesture.SwipeLeft:
                case NuimoGesture.SwipeRight:
                    var newRoom = SwitchRoom(nuimoGestureEvent.Gesture);
                    var matrixForRoom = MatrixForRoomClass(newRoom);
                    controller.DisplayLedMatrixAsync(matrixForRoom);
                    break;
                case NuimoGesture.SwipeUp:
                    break;
                case NuimoGesture.SwipeDown:
                    break;
                case NuimoGesture.FlyLeft:
                    break;
                case NuimoGesture.FlyRight:
                    break;
                case NuimoGesture.FlyBackwards:
                    break;
                case NuimoGesture.FlyTowards:
                    break;
                case NuimoGesture.FlyUpDown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task SetupHue()
        {
            await SetupHueClient();

            var groups = await HueClient.GetGroupsAsync();

            FillGroupDictionary(groups);

            OnOffStateForGroups =
                groups.ToDictionary(group => group, group => group.State.AnyOn == true);

            Rooms = new LinkedList<RoomClass>(GroupDictionary.Keys);
            CurrentRoom = Rooms.Find(RoomClass.LivingRoom);
        }

        private async Task SetupHueClient()
        {
            ////todo detect hue bridge and store settings in ApplicationDataContainer
            //var uri = new System.Uri("ms-appx:///hueSettings.json");
            //var settingsFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
            //var settingsContent = await Windows.Storage.FileIO.ReadTextAsync(settingsFile);

            //var hueSettings = JsonConvert.DeserializeObject<HueSettings>(settingsContent);

            var hueSettings = new HueSettings
            {
                Ip = "192.168.0.1",
                Username = "putYourHueUsernameHere"
            };
            HueClient = new LocalHueClient(hueSettings.Ip, hueSettings.Username);
            var test = HueClient.IsInitialized;
        }

        private void FillGroupDictionary(IReadOnlyCollection<Group> groups)
        {
            var availableRoomClasses = groups
                .Where(group => group.Class != null)
                .Select(group => group.Class)
                .Distinct();

            foreach (var roomClass in availableRoomClasses)
                if (roomClass != null)
                {
                    var groupForClass = groups.FirstOrDefault(group => group.Class == roomClass);
                    GroupDictionary.Add(roomClass.Value, groupForClass);
                }
        }

        private RoomClass SwitchRoom(NuimoGesture gesture)
        {
            switch (gesture)
            {
                case NuimoGesture.SwipeLeft:
                    CurrentRoom = CurrentRoom?.PreviousOrLast();
                    break;
                case NuimoGesture.SwipeRight:
                    CurrentRoom = CurrentRoom?.NextOrFirst();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gesture), gesture, null);
            }
            return CurrentRoom.Value;
        }

        private bool SwitchGroupOnOff()
        {
            var currentGroup = GroupDictionary[CurrentRoom.Value];
            var currentState = OnOffStateForGroups[currentGroup];
            var newState = !currentState;
            var command = new LightCommand {On = newState};
            HueClient.SendGroupCommandAsync(command, currentGroup.Id);
            return newState;
        }

        private byte ChangeBrightness(int value)
        {
            var currentGroup = GroupDictionary[CurrentRoom.Value];

            var currentBrightness = currentGroup.Action.Brightness;
            var delta = 5 * GetSign(value);
            var newBrightness = currentBrightness + (byte) delta;
            var command = new LightCommand {BrightnessIncrement = delta};

            var currentState = OnOffStateForGroups[currentGroup];
            if (!currentState)
                command.On = true;
            if (newBrightness <= 0)
                command.On = false;

            HueClient.SendGroupCommandAsync(command, currentGroup.Id);
            return (byte) newBrightness;
        }

        private NuimoLedMatrix MatrixForRoomClass(RoomClass roomClass)
        {
            switch (roomClass)
            {
                case RoomClass.LivingRoom:
                    return Icons.Couch;
                case RoomClass.Bedroom:
                    return Icons.Bed;
                case RoomClass.Hallway:
                    return Icons.Door;
                default:
                    return Icons.Home;
            }
        }

        private NuimoLedMatrix GetPowerMatrix(bool on)
        {
            if (on)
                return Icons.PowerOn;
            return Icons.PowerOff;
        }

        private int GetSign(int value)
        {
            if (value > 0)
                return 1;
            if (value < 0)
                return -1;
            return 0;
        }
    }

    public static class RoomCaroussel
    {
        public static LinkedListNode<RoomClass> NextOrFirst(this LinkedListNode<RoomClass> current)
        {
            if (current.Next == null)
                return current.List.First;
            return current.Next;
        }

        public static LinkedListNode<RoomClass> PreviousOrLast(this LinkedListNode<RoomClass> current)
        {
            if (current.Previous == null)
                return current.List.Last;
            return current.Previous;
        }
    }
}
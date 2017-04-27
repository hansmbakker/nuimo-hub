using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuimoHelpers.LedMatrices;
using NuimoHub.Core.Configuration;
using NuimoHub.Interfaces;
using NuimoSDK;
using Q42.HueApi;
using Q42.HueApi.Models.Groups;

namespace HueApp
{
    public class HueApp : INuimoApp
    {
        private readonly HueOptions _hueOptions;

        public HueApp(HueOptions hueOptions)
        {
            _hueOptions = hueOptions;
        }

        private HueClient HueClient { get; set; }

        private bool IsInitialized = false;

        private Dictionary<RoomClass, Group> GroupDictionary { get; set; }

        private LinkedList<RoomClass> Rooms { get; set; }
        private LinkedListNode<RoomClass> CurrentRoom { get; set; }

        private Dictionary<Group, bool> OnOffStateForGroups { get; set; }
        public string Name => "Hue";

        public NuimoLedMatrix Icon => Icons.LightBulb;

        public void OnFocus(INuimoController nuimoController)
        {
            if (!IsInitialized)
            {
                nuimoController.DisplayLedMatrixAsync(NuimoBuiltinLedMatrix.Busy, 5);
                SetupHue().Wait();
                nuimoController.DisplayLedMatrixAsync(new NuimoLedMatrix());
            }
        }

        public void OnLostFocus(INuimoController sender)
        {
            //throw new NotImplementedException();
        }

        public void OnGestureEvent(INuimoController controller, NuimoGestureEvent nuimoGestureEvent)
        {
            if (!IsInitialized)
            {
                controller.DisplayLedMatrixAsync(Icons.Cross);
                return;
            }

            switch (nuimoGestureEvent.Gesture)
            {
                case NuimoGesture.ButtonPress:
                    var newState = SwitchGroupOnOff();
                    var matrix = GetPowerMatrix(newState);
                    controller.DisplayLedMatrixAsync(matrix);
                    break;
                case NuimoGesture.Rotate:
                    var newBrightness = ChangeBrightness(nuimoGestureEvent.Value) / 255.0;
                    var brightnessMatrix = ProgressBars.VerticalBar(newBrightness);
                    controller.DisplayLedMatrixAsync(brightnessMatrix);
                    break;
                case NuimoGesture.SwipeLeft:
                case NuimoGesture.SwipeRight:
                    var newRoom = SwitchRoom(nuimoGestureEvent.Gesture);
                    var matrixForRoom = MatrixForRoomClass(newRoom);
                    controller.DisplayLedMatrixAsync(matrixForRoom);
                    break;
                default:
                    break;
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
            if (CurrentRoom == null)
            {
                CurrentRoom = Rooms.First;
            }
            IsInitialized = true;
        }

        private async Task SetupHueClient()
        {
            var bridge = _hueOptions.Bridge;
            HueClient = new LocalHueClient(bridge.Ip, bridge.AppKey);
        }

        private void FillGroupDictionary(IReadOnlyCollection<Group> groups)
        {
            var availableRoomClasses = groups
                .Where(group => group.Class != null)
                .Select(group => group.Class)
                .Distinct();

            GroupDictionary = availableRoomClasses
                .Select(roomClass => groups.FirstOrDefault(group => group.Class == roomClass))
                .ToDictionary(group => (RoomClass)group.Class);
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
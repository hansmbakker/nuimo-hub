using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NuimoHelpers;
using NuimoHub.Interfaces;
using NuimoSDK;

namespace NuimoHub
{
    internal class NuimoHub : INuimoHub
    {
        private readonly PairedNuimoManager _pairedNuimoManager = new PairedNuimoManager();

        private readonly LinkedList<INuimoApp> Applications;

        public NuimoHub()
        {
            var apps = new List<INuimoApp>
            {
                //new CastApp.CastApp(),
                new HueApp.HueApp(),
                new TimerApp.TimerApp(),
                new TestApp.TestApp()
            };
            Applications = new LinkedList<INuimoApp>(apps);
        }

        private LinkedListNode<INuimoApp> CurrentApp { get; set; }

        public async void Start()
        {
            _pairedNuimoManager.NuimoFound += _pairedNuimoManager_NuimoFound;
            ;
            _pairedNuimoManager.StartWatching();

            await Task.Delay(5000);
            _pairedNuimoManager.StopWatching();
        }

        private async void _pairedNuimoManager_NuimoFound(INuimoController nuimoController)
        {
            await HandleNewNuimoController(nuimoController);
        }

        private async Task HandleNewNuimoController(INuimoController nuimoController)
        {
            AddDelegates(nuimoController);
            while (!await nuimoController.ConnectAsync())
            {
            }

            nuimoController.HeartBeatInterval = TimeSpan.FromSeconds(2);
            NextApp(nuimoController);
        }

        private void AddDelegates(INuimoController controller)
        {
            if (controller == null)
                return;

            controller.FirmwareVersionRead += OnFirmwareVersion;
            controller.HardwareVersionRead += OnHardwareVersion;
            controller.ColorRead += OnColorRead;

            controller.BatteryPercentageChanged += OnBatteryPercentage;
            controller.LedMatrixDisplayed += OnLedMatrixDisplayed;

            controller.GestureEventOccurred += OnNuimoGestureEvent;

            controller.ConnectionStateChanged += OnConnectionState;
            controller.HeartbeatReceived += OnHeartbeatReceived;


        }

        private void OnHeartbeatReceived(INuimoController nuimoController, object heartbeatObject)
        {
            Debug.WriteLine("Heartbeat received");
        }

        private void NextApp(INuimoController controller)
        {
            var currentApp = CurrentApp?.Value;
            if (currentApp != null)
            {
                controller.ThrottledGestureEventOccurred -= currentApp.OnGestureEvent;
                currentApp.OnLostFocus(controller);
            }
            else
            {
                //Set to Last so that when calling NextOrFirst, the first app will be loaded
                CurrentApp = Applications.Last;
            }

            var newAppNode = CurrentApp?.NextOrFirst();
            var newApp = newAppNode?.Value;
            if (newApp != null)
            {
                controller.ThrottledGestureEventOccurred += newApp.OnGestureEvent;

                controller.DisplayLedMatrixAsync(newApp.Icon);
                newApp.OnFocus(controller);
                Debug.WriteLine($"Switched to {newApp.Name}");
            }

            CurrentApp = newAppNode;
        }

        #region delegates

        private void OnNuimoGestureEvent(INuimoController nuimoController, NuimoGestureEvent nuimoGestureEvent)
        {
            Debug.WriteLine("Event: " + nuimoGestureEvent.Gesture + ", " + nuimoGestureEvent.Value);

            if (nuimoGestureEvent.Gesture == NuimoGesture.SwipeDown)
                NextApp(nuimoController);
        }

        private void OnFirmwareVersion(INuimoController nuimoController, string firmwareVersion)
        {
            Debug.WriteLine(firmwareVersion);
        }

        private void OnColorRead(INuimoController nuimoController, NuimoColor color)
        {
            Debug.WriteLine(color);
        }

        private void OnHardwareVersion(INuimoController nuimoController, string hardwareVersion)
        {
            Debug.WriteLine(hardwareVersion);
        }

        private void OnConnectionState(INuimoController nuimoController, NuimoConnectionState nuimoConnectionState)
        {
            Debug.WriteLine("Connection state: " + nuimoConnectionState);
        }

        private void OnBatteryPercentage(INuimoController nuimoController, int batteryPercentage)
        {
            Debug.WriteLine("Battery percentage: " + batteryPercentage);
        }

        private void OnLedMatrixDisplayed(INuimoController nuimoController)
        {
            Debug.WriteLine("LED matrix displayed");
        }

        #endregion
    }
}
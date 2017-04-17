using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NuimoHelpers.LedMatrices;
using NuimoHub.Interfaces;
using NuimoSDK;

namespace TimerApp
{
    public class TimerApp : INuimoApp
    {
        public TimerApp()
        {
            TimeLeft = TimeSpan.Zero;
        }

        private TimeSpan TimeLeft { get; set; }

        private Timer Timer { get; set; }

        public bool HasFocus { get; set; }

        public bool TimerIsRunning { get; set; }
        public string Name => "Timer";
        public NuimoLedMatrix Icon => Icons.Timer;

        public void OnFocus(INuimoHub sender)
        {
            HasFocus = true;
        }

        public void OnLostFocus(INuimoHub sender)
        {
            HasFocus = false;
        }

        public void OnGestureEvent(INuimoController sender, NuimoGestureEvent nuimoGestureEvent)
        {
            switch (nuimoGestureEvent.Gesture)
            {
                case NuimoGesture.Rotate:
                    ChangeTime(nuimoGestureEvent.Value);
                    ShowTime(sender);
                    break;
                case NuimoGesture.ButtonPress:
                    SetUnset(sender);
                    break;
            }
        }

        private void SetUnset(INuimoController controller)
        {
            if (TimerIsRunning)
            {
                StopCountdown();
            }
            else
            {
                Timer = new Timer(ProcessCountdownTick, controller, TimeSpan.Zero, TimeSpan.FromSeconds(1));
                TimerIsRunning = true;
            }
        }

        private void StopCountdown()
        {
            Timer.Dispose();
            TimeLeft = TimeSpan.Zero;
            TimerIsRunning = false;
        }

        private void ProcessCountdownTick(object controllerObject)
        {
            var controller = controllerObject as INuimoController;
            ;

            TimeLeft -= TimeSpan.FromSeconds(1);

            if (TimeLeft < TimeSpan.Zero)
            {
                StopCountdown();
                if (controller != null)
                    ShowTimeoutAlert(controller);
            }
            else
            {
                if (controller != null && HasFocus)
                    ShowTime(controller);
            }
        }

        private async Task ShowTimeoutAlert(INuimoController controller)
        {
            for (var i = 0; i < 10; i++)
            {
                var durationSeconds = 0.7;
                controller.DisplayLedMatrixAsync(Icons.Timer, durationSeconds,
                    NuimoLedMatrixWriteOptions.WithoutWriteResponse);
                await Task.Delay(TimeSpan.FromSeconds(durationSeconds));
            }
        }

        private void ChangeTime(int value)
        {
            //reset if more than 3 hours and adding
            if (TimeLeft > TimeSpan.FromHours(3) && value > 0)
                TimeLeft = TimeSpan.MinValue;
            else if (TimeLeft > TimeSpan.FromHours(1))
                TimeLeft += TimeSpan.FromMinutes(30 * GetSign(value));
            else if (TimeLeft > TimeSpan.FromMinutes(1))
                TimeLeft += TimeSpan.FromMinutes(1 * GetSign(value));
            else if (TimeLeft > TimeSpan.FromSeconds(10))
                TimeLeft += TimeSpan.FromSeconds(10 * GetSign(value));
            else
                TimeLeft += TimeSpan.FromSeconds(1 * GetSign(value));

            if (TimeLeft < TimeSpan.Zero)
                TimeLeft = TimeSpan.Zero;
        }

        private void ShowTime(INuimoController controller)
        {
            var timeDisplay = new NuimoLedMatrix("");
            if (TimeLeft >= TimeSpan.FromHours(1))
                timeDisplay = timeDisplay.AddHours(TimeLeft.Hours);

            if (TimeLeft >= TimeSpan.FromMinutes(1))
                timeDisplay = timeDisplay.AddMinutes(TimeLeft.Minutes);
            else
                timeDisplay = timeDisplay.AddSeconds(TimeLeft.Seconds);

            controller?.DisplayLedMatrixAsync(timeDisplay, 2, NuimoLedMatrixWriteOptions.WithoutWriteResponse);
            Debug.WriteLine(TimeLeft);
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
}
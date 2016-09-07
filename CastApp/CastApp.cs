using System;
using System.Diagnostics;
using NuimoHelpers.LedMatrices;
using NuimoHub.Interfaces;
using NuimoSDK;
using SharpCaster.Interfaces;
using SharpCaster.Models.ChromecastStatus;
using SharpCaster.Models.MediaStatus;
using SharpCaster.Services;

namespace CastApp
{
    public class CastApp : INuimoApp
    {
        private static readonly ChromecastService ChromecastService = ChromecastService.Current;

        public CastApp()
        {
#pragma warning disable 4014
            var chromecasts = ChromecastService.Current.StartLocatingDevices().Result;
            Debug.WriteLine("Started locating chromecasts!");
#pragma warning restore 4014
            ChromecastService.ChromeCastClient.ApplicationStarted += Client_ApplicationStarted;
            //ChromecastService.ChromeCastClient.VolumeChanged += _client_VolumeChanged;
            ChromecastService.ChromeCastClient.MediaStatusChanged += ChromeCastClient_MediaStatusChanged;
            ChromecastService.ChromeCastClient.ConnectedChanged += ChromeCastClient_Connected;
        }

        private MediaStatus MediaStatus { get; set; }

        private IMediaController MediaController { get; set; }
        public string Name => "Google Cast";
        public NuimoLedMatrix Icon => Icons.Cast;

        public void OnFocus(INuimoHub nuimoHub)
        {
            //throw new NotImplementedException();
        }

        public void OnLostFocus(INuimoHub sender)
        {
            throw new NotImplementedException();
        }

        public void OnGestureEvent(INuimoController sender, NuimoGestureEvent nuimoGestureEvent)
        {
            switch (nuimoGestureEvent.Gesture)
            {
                case NuimoGesture.ButtonPress:
                    PlayPause();
                    break;
                case NuimoGesture.Rotate:
                    ChangeVolume(nuimoGestureEvent.Value);
                    break;
                case NuimoGesture.SwipeLeft:
                    Previous();
                    break;
                case NuimoGesture.SwipeRight:
                    Next();
                    break;
            }
        }

        private void ChromeCastClient_MediaStatusChanged(object sender, MediaStatus e)
        {
            MediaStatus = e;
        }

        private void ChromeCastClient_Connected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_ApplicationStarted(object sender, ChromecastApplication e)
        {
            throw new NotImplementedException();
        }

        private void Next()
        {
            MediaController.Next();
        }

        private void Previous()
        {
            MediaController.Previous();
        }

        private void ChangeVolume(int value)
        {
            if (value < 0)
                MediaController.VolumeDown();
            else
                MediaController.VolumeUp();
        }

        private void PlayPause()
        {
            switch (MediaStatus?.PlayerState)
            {
                case PlayerState.Paused:
                    MediaController?.Play();
                    break;

                case PlayerState.Playing:
                    MediaController?.Pause();
                    break;

                default:
                    break;
            }
        }
    }
}
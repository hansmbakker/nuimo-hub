using System;
using System.Diagnostics;
using NuimoHelpers.LedMatrices;
using NuimoHub.Interfaces;
using NuimoSDK;

namespace TestApp
{
    public class TestApp : INuimoApp
    {
        private double _value;

        private double TestValue
        {
            get { return _value; }
            set
            {
                if (value < 0)
                    _value = 0;
                else if (value > 1.0)
                    _value = 1.0;
                else
                    _value = value;
            }
        }

        public string Name => "TestApp";

        public NuimoLedMatrix Icon => Characters.LetterT;

        public void OnGestureEvent(INuimoController sender, NuimoGestureEvent nuimoGestureEvent)
        {
            if (nuimoGestureEvent.Gesture == NuimoGesture.Rotate)
            {
                if (nuimoGestureEvent.Value > 0)
                    TestValue += 0.1;
                else if (nuimoGestureEvent.Value < 0)
                    TestValue -= 0.1;
                var progressMatrix = ProgressBars.VerticalBar(TestValue);
                sender.DisplayLedMatrixAsync(progressMatrix, 2,
                    NuimoLedMatrixWriteOptions.WithFadeTransition | NuimoLedMatrixWriteOptions.WithoutWriteResponse);
            }
            Debug.WriteLine(nuimoGestureEvent.Gesture);
        }

        public void OnLostFocus(INuimoHub sender)
        {
            //throw new NotImplementedException();
        }

        public void OnFocus(INuimoHub sender)
        {
            //throw new NotImplementedException();
        }
    }
}
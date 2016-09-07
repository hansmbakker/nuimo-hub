using NuimoSDK;

namespace NuimoHub.Interfaces
{
    public interface INuimoApp
    {
        string Name { get; }

        NuimoLedMatrix Icon { get; }

        void OnFocus(INuimoHub sender);

        void OnLostFocus(INuimoHub sender);

        void OnGestureEvent(INuimoController sender, NuimoGestureEvent nuimoGestureEvent);
    }
}
using NuimoSDK;

namespace NuimoHub.Interfaces
{
    public interface INuimoApp
    {
        string Name { get; }

        NuimoLedMatrix Icon { get; }

        void OnFocus(INuimoController sender);

        void OnLostFocus(INuimoController sender);

        void OnGestureEvent(INuimoController sender, NuimoGestureEvent nuimoGestureEvent);
    }
}
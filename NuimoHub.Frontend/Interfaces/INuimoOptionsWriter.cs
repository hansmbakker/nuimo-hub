using NuimoHub.Frontend.Models;

namespace NuimoHub.Frontend.Interfaces
{
    public interface INuimoOptionsWriter
    {
        void AddNuimo(Nuimo nuimo);
        void RemoveNuimo(Nuimo nuimo);

        void SetHueOptions(HueOptions hueOptions);

        void SetChromecastOptions(ChromecastOptions chromecastOptions);
    }
}
using System.Collections.Generic;

namespace NuimoHub.Core.Configuration
{
    public class NuimoOptions
    {
        public NuimoOptions()
        {
            WhitelistedNuimos = new List<Nuimo>();
        }

        public HueOptions HueOptions { get; set; }
        public ChromecastOptions ChromecastOptions { get; set; }
        public List<Nuimo> WhitelistedNuimos { get; set; }
    }
}
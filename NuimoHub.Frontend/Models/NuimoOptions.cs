using System.Collections.Generic;

namespace NuimoHub.Frontend.Models
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

    public class HueOptions
    {
        public HueBridge Bridge { get; set; }

        public string SceneId1 { get; set; }
        public string SceneId2 { get; set; }
        public string SceneId3 { get; set; }
    }

    public class ChromecastOptions
    {
        public string Ip { get; set; }
        public string Name { get; set; }
    }
}
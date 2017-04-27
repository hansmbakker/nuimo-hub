using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NuimoHub.Core.Configuration;

namespace NuimoHub
{
    sealed class OptionsDownloader
    {
        public static async Task<NuimoOptions> GetOptions()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000");
                var response = await client.GetAsync("/api/Configuration");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var options = await response.Content.ReadAsAsync<NuimoOptions>();
                return options;
            }
        }
    }
}

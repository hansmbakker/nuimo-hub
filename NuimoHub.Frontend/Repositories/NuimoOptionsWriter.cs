using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NuimoHub.Core.Configuration;
using NuimoHub.Frontend.Interfaces;

namespace NuimoHub.Frontend.Repositories
{
    public class NuimoOptionsWriter : INuimoOptionsWriter
    {
        private readonly NuimoOptions _nuimoOptions;
        private readonly IFileProvider _fileProvider;

        private Object lockObject = new Object();

        public NuimoOptionsWriter(IOptionsSnapshot<NuimoOptions> nuimoOptions, IFileProvider fileProvider)
        {
            _nuimoOptions = nuimoOptions.Value;
            _fileProvider = fileProvider;
        }

        public void AddNuimo(Nuimo nuimo)
        {
            if(nuimo != null)
            {
                var newOptions = _nuimoOptions;
                newOptions.WhitelistedNuimos.Add(nuimo);
                StoreNewNuimoOptions(newOptions);
            }
        }

        public void RemoveNuimo(Nuimo nuimo)
        {
            if(nuimo != null)
            {
                var newOptions = _nuimoOptions;
                newOptions.WhitelistedNuimos.Remove(nuimo);
                StoreNewNuimoOptions(newOptions);
            }
        }

        public void SetHueOptions(HueOptions hueOptions)
        {
            var newOptions = _nuimoOptions;
            newOptions.HueOptions = hueOptions;
            StoreNewNuimoOptions(newOptions);
        }

        public void SetChromecastOptions(ChromecastOptions chromecastOptions)
        {
            var newOptions = _nuimoOptions;
            newOptions.ChromecastOptions = chromecastOptions;
            StoreNewNuimoOptions(newOptions);
        }
        
        private void StoreNewNuimoOptions(NuimoOptions newOptions)
        {
            lock(lockObject)
            {
                var file = _fileProvider.GetFileInfo("appSettings.json");
                var settingsJson = System.IO.File.ReadAllText(file.PhysicalPath);
                dynamic settings = JsonConvert.DeserializeObject(settingsJson);

                settings["NuimoOptions"] = JToken.FromObject(newOptions);

                settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);

                System.IO.File.WriteAllText(file.PhysicalPath, settingsJson);
            }
        }
    }
}
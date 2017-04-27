using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using NuimoHub.Frontend.Interfaces;
using NuimoHub.Frontend.Models;

using Rssdp;

namespace NuimoHub.Frontend.Controllers
{
    [Route("api/[controller]")]
    public class ChromecastController : Controller
    {
        private readonly NuimoOptions _nuimoOptions;
        private readonly INuimoOptionsWriter _nuimoOptionsWriter;

        public ChromecastController(IOptionsSnapshot<NuimoOptions> nuimoOptions, INuimoOptionsWriter nuimoOptionsWriter)
        {
            _nuimoOptions = nuimoOptions.Value;
            _nuimoOptionsWriter = nuimoOptionsWriter;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(_nuimoOptions.ChromecastOptions);
        }

        [HttpPost("Discover")]
        public async Task<IActionResult> Discover()
        {
            using (var deviceLocator = new SsdpDeviceLocator())
            {
                var deviceType = "urn:dial-multiscreen-org:device:dial:1";
                deviceLocator.NotificationFilter = deviceType;
                                
                var foundDevices = await deviceLocator.SearchAsync(deviceType);

                if(foundDevices.Any())
                {
                    var getChromecastTasks = foundDevices
                            .Select(device => GetChromecastDetails(device));
                    
                    var chromecasts = await Task.WhenAll(getChromecastTasks);

                    return Ok(chromecasts);
                }
            }

            return NotFound();            
        }

        [HttpPost("Select")]
        public IActionResult SelectChromecast([FromBody]ChromecastOptions chromecast)
        {
            if(chromecast == null || string.IsNullOrWhiteSpace(chromecast.Ip))
            {
                return BadRequest("invalid chromecast settings");
            }
            
            _nuimoOptionsWriter.SetChromecastOptions(chromecast);
            
            return Ok($"Chromecast \"{chromecast.Name}\" at {chromecast.Ip}");
        }

        [HttpDelete("")]
        public IActionResult Delete()
        {
            _nuimoOptionsWriter.SetChromecastOptions(null);
            return Ok($"Chromecast settings are reset");            
        }

        private async Task<ChromecastOptions> GetChromecastDetails(DiscoveredSsdpDevice foundDevice)
        {
            if(foundDevice == null)
            {
                return null;
            }

            var fullDevice = await foundDevice.GetDeviceInfo();

            var chromecast = new ChromecastOptions
            {
                Ip = foundDevice.DescriptionLocation.Host,
                Name = fullDevice.FriendlyName
            };

            return chromecast;
        }

    }
}

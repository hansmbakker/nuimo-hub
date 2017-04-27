using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using NuimoHub.Frontend.Interfaces;
using NuimoHub.Frontend.Models;

using Q42.HueApi;


namespace NuimoHub.Frontend.Controllers
{
    [Route("api/[controller]")]
    public class HueController : Controller
    {
        private readonly NuimoOptions _nuimoOptions;
        private readonly INuimoOptionsWriter _nuimoOptionsWriter;
        
        public HueController(IOptionsSnapshot<NuimoOptions> nuimoOptions, INuimoOptionsWriter nuimoOptionsWriter)
        {
            _nuimoOptions = nuimoOptions.Value;
            _nuimoOptionsWriter = nuimoOptionsWriter;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(_nuimoOptions.HueOptions);
        }

        [HttpPost("Discover")]
        public async Task<IActionResult> Discover()
        {
            var locator = new HttpBridgeLocator();

            //For Windows 8 and .NET45 projects you can use the SSDPBridgeLocator which actually scans your network. 
            //See the included BridgeDiscoveryTests and the specific .NET and .WinRT projects
            var bridges = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

            if(bridges.Any())
            {
                var hueOptionsList = bridges.Select(bridge => new HueBridge
                {
                    Ip = bridge.IpAddress
                });

                return Ok(hueOptionsList);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("Select")]
        public async Task<IActionResult> SelectBridge([FromBody]HueBridge hueBridge)
        {
            if(hueBridge == null || string.IsNullOrWhiteSpace(hueBridge.Ip))
            {
                return BadRequest("invalid hue bridge settings");
            }

            var client = new LocalHueClient(hueBridge.Ip);
            for(int i = 0; i <= 10; i++)
            {
                try
                {
                    var appKey = await client.RegisterAsync("NuimoHub", Environment.GetEnvironmentVariable("COMPUTERNAME"));
                    hueBridge.AppKey = appKey;

                    var newHueOptions = new HueOptions {
                        Bridge = hueBridge
                    };

                    _nuimoOptionsWriter.SetHueOptions(newHueOptions);

                    return Ok($"hue bridge at {hueBridge.Ip} added; appKey is {appKey}");
                }
                catch (System.Exception)
                {
                    Debug.WriteLine("Button was not pressed.");
                    await Task.Delay(3000);
                }
            }
            
            return Unauthorized();
        }

        [HttpDelete("")]
        public IActionResult Delete()
        {
            _nuimoOptionsWriter.SetHueOptions(null);
            return Ok($"Hue settings are reset");            
        }

    }
}

using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using NuimoHub.Core.Configuration;
using NuimoHub.Frontend.Interfaces;


namespace NuimoHub.Frontend.Controllers
{
    [Route("api/[controller]")]
    public class NuimoController : Controller
    {
        private readonly NuimoOptions _nuimoOptions;
        private readonly INuimoOptionsWriter _nuimoOptionsWriter;
        
        public NuimoController(IOptionsSnapshot<NuimoOptions> nuimoOptions, INuimoOptionsWriter nuimoOptionsWriter)
        {
            _nuimoOptions = nuimoOptions.Value;
            _nuimoOptionsWriter = nuimoOptionsWriter;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(_nuimoOptions.WhitelistedNuimos);
        }

        [HttpPost("")]
        public IActionResult Post([FromBody]Nuimo nuimo)
        {
            if(nuimo == null)
            {
                return BadRequest("invalid nuimo object");
            }
            
            if(!CheckDeviceId(nuimo.DeviceId))
            {
                return BadRequest("invalid id");
            }

            if(!IsKnownNuimo(nuimo.DeviceId)){
                _nuimoOptionsWriter.AddNuimo(nuimo);
                return Ok($"Nuimo {nuimo.Name} added");
            }
            else
            {
                return BadRequest($"{nuimo.Name} is already added.");
            }
        }

        [HttpDelete("")]
        public IActionResult Delete([FromBody]Nuimo nuimo)
        {
            if(nuimo == null)
            {
                return BadRequest("invalid nuimo object");
            }

            if(!CheckDeviceId(nuimo.DeviceId))
            {
                return BadRequest("invalid id");
            }

            if(!IsKnownNuimo(nuimo.DeviceId)){
                return NotFound($"Nuimo with device id {nuimo.DeviceId} is unknown.");                
            }
            else
            {
                _nuimoOptionsWriter.RemoveNuimo(nuimo);
                return Ok($"Nuimo with device id {nuimo.DeviceId} removed");
            }
        }

        private bool CheckDeviceId(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            var regex = new Regex("^BluetoothLE#BluetoothLE([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})-([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$");
            var match = regex.Match(id);
            return match.Success;
        }

        private bool IsKnownNuimo(string deviceId)
        {
            var knownDeviceIds = _nuimoOptions
                        .WhitelistedNuimos
                        .Select(nuimoItem => nuimoItem.DeviceId)
                        .Distinct().ToList();

            return knownDeviceIds.Contains(deviceId);
        }
    }
}

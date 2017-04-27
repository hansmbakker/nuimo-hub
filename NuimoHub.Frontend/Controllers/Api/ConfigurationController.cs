using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using NuimoHub.Core.Configuration;
using NuimoHub.Frontend.Extensions;


namespace NuimoHub.Frontend.Controllers
{
    [Route("api/[controller]")]
    public class ConfigurationController : Controller
    {
        private readonly NuimoOptions _nuimoOptions;

        public ConfigurationController(IOptionsSnapshot<NuimoOptions> nuimoOptions)
        {
            _nuimoOptions = nuimoOptions.Value;
        }

        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Request.IsLocal())
            {
                return Ok(_nuimoOptions);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}

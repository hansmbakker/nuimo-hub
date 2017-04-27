using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

using NuimoHub.Core.Configuration;


namespace NuimoHub.Frontend.Controllers
{
    public class ConfigureController : Controller
    {
        private readonly NuimoOptions _nuimoOptions;

        public ConfigureController(IOptionsSnapshot<NuimoOptions> nuimoOptions)
        {
            _nuimoOptions = nuimoOptions.Value;
        }

        public IActionResult Index()
        {
            ViewData["Configuration"] = JsonConvert.SerializeObject(_nuimoOptions, Formatting.Indented);
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

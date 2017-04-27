using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NuimoHub.Frontend.Models;


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

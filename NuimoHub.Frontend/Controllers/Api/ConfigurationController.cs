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

using NuimoHub.Frontend.Extensions;
using NuimoHub.Frontend.Models;


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

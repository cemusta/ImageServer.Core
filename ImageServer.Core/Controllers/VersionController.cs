using System.Collections.Generic;
using System.Linq;
using ImageServer.Core.Model;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageServer.Core.Controllers
{
    [Route("")]
    public class VersionController : Controller
    {
        private readonly IConfiguration _iconfiguration;
        private readonly IImageService _imageService;
        private readonly ILogger<VersionController> _logger;
        private readonly List<HostConfig> _hosts;


        public VersionController(IConfiguration iconfiguration, IImageService imageService, ILogger<VersionController> logger, IOptions<List<HostConfig>> hosts)
        {
            _iconfiguration = iconfiguration;
            _imageService = imageService;
            _logger = logger;
            _hosts = hosts.Value;
        }

        // GET status
        [Route("/status")]
        [HttpGet]
        public JsonResult Status()
        {
            _logger.LogInformation("Status requested");

            return Json(new { Status = "ok" });
        }

        [HttpGet("/ver")]
        [HttpGet("/version")]
        public JsonResult ReturnVersion()
        {
            _logger.LogInformation("Version requested");
            
            var verName = _iconfiguration["App:VersionName"] ?? "unknown";
            var build = _iconfiguration["App:Build"] ?? "unknown";
            var magickNet = new { version = _imageService.GetVersion(), features = _imageService.GetFeatures() };

            return Json(new { version=$"{build} aka {verName}", magickNet });
        }


        [HttpGet("/formats")]
        public JsonResult ReturnImageFormats()
        {
            _logger.LogInformation("Formats requested");

            var magickNet = new { formats = _imageService.GetSupportedFormats() };

            return Json(new { magickNet });
        }

        [HttpGet("/hosts")]
        public JsonResult ReturnImageHosts()
        {
            _logger.LogInformation("Hosts requested");

            var hosts = _hosts.Select(x => $"{x.Slug} ({x.Type})");

            return Json(new { hosts });
        }


    }
}
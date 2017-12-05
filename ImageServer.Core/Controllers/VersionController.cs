using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ImageServer.Core.Controllers
{
    [Route("")]
    public class VersionController : Controller
    {
        private readonly IConfiguration _iconfiguration;
        private readonly IImageService _imageService;
        private readonly ILogger<VersionController> _logger;

        public VersionController(IConfiguration iconfiguration, IImageService imageService, ILogger<VersionController> logger)
        {
            _iconfiguration = iconfiguration;
            _imageService = imageService;
            _logger = logger;
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
            
            var ver = _iconfiguration["App:Version"] ?? "unknown";
            var build = _iconfiguration["App:Build"] ?? "unknown";
            var magickNet = new { ver = _imageService.GetVersion(), features = _imageService.GetFeatures(), formats = _imageService.GetSupportedFormats() };

            return Json(new { ver, build, magickNet });
        }
    }
}
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImageServer.Core.Controllers
{
    [Route("")]
    public class VersionController : Controller
    {
        readonly IConfiguration _iconfiguration;
        private readonly IImageService _imageService;
        public VersionController(IConfiguration iconfiguration, IImageService imageService)
        {
            _iconfiguration = iconfiguration;
            _imageService = imageService;
        }

        // GET status
        [Route("/status")]
        [HttpGet]
        public JsonResult Status()
        {
            return Json(new { Status = "ok" });
        }

        [HttpGet("/ver")]
        [HttpGet("/version")]
        public JsonResult ReturnVersion()
        {
            var ver = _iconfiguration["App:Version"] ?? "unknown";
            var build = _iconfiguration["App:Build"] ?? "unknown";
            var magickNet = new { ver = _imageService.GetVersion(), features = _imageService.GetFeatures(), formats = _imageService.GetSupportedFormats() };

            return Json(new { ver, build, magickNet });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImageServer.Core.Controllers
{
    [Route("")]
    public class VersionController : Controller
    {
        readonly IConfiguration _iconfiguration;
        public VersionController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
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

            return Json(new { ver, build });
        }
    }
}
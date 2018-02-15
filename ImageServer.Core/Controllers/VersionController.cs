using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public VersionController(IConfiguration iconfiguration, IImageService imageService, ILogger<VersionController> logger, IOptions<List<HostConfig>> hosts, IMemoryCache memoryCache)
        {
            _iconfiguration = iconfiguration ?? throw new ArgumentNullException(nameof(iconfiguration));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hosts = hosts.Value ?? throw new ArgumentNullException(nameof(hosts));
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        // GET status
        [Route("/status")]
        [HttpGet]
        public async Task<IActionResult> Status()
        {
            var cacheEntry = await
                _cache.GetOrCreateAsync("ver:status", entry =>
                {
                    _logger.LogInformation("Status requested and cached.");

                    return Task.FromResult(new { Status = "ok" });
                });

            return Json(cacheEntry);
        }

        [HttpGet("/ver")]
        [HttpGet("/version")]
        public async Task<IActionResult> ReturnVersion()
        {
            var cacheEntry = await
                _cache.GetOrCreateAsync("ver:version", entry =>
                {
                    _logger.LogInformation("Version requested and cached.");

                    var verName = _iconfiguration["App:VersionName"] ?? "unknown";
                    var build = _iconfiguration["App:Build"] ?? "unknown";
                    var magickNet = new { version = _imageService.GetVersion(), features = _imageService.GetFeatures() };

                    return Task.FromResult(new { version = $"{build} aka {verName}", magickNet });
                });

            return Json(cacheEntry);
        }

        [HttpGet("/formats")]
        public async Task<IActionResult> ReturnImageFormats()
        {
            var cacheEntry = await
                _cache.GetOrCreateAsync("ver:formats", entry =>
                {
                    _logger.LogInformation("Formats requested and cached.");

                    var magickNet = new { formats = _imageService.GetSupportedFormats() };
                    return Task.FromResult(magickNet);
                });

            return Json(cacheEntry);
        }

        [HttpGet("/hosts")]
        public async Task<IActionResult> ReturnImageHosts()
        {
            var cacheEntry = await
                _cache.GetOrCreateAsync("ver:hosts", entry =>
                {
                    _logger.LogInformation("Hosts requested and cached.");
                    var hosts = _hosts.Select(x => $"{x.Slug} ({x.Type})" + (string.IsNullOrEmpty(x.Path) ? "" : $" : {x.Path}") + (string.IsNullOrEmpty(x.DatabaseName) ? "" : $" : {x.DatabaseName}") + (string.IsNullOrEmpty(x.Backend) ? "" : $" : {x.Backend}"));
                    return Task.FromResult(hosts);
                });

            return Json(cacheEntry);
        }

    }
}
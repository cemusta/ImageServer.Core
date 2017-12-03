using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageServer.Core.Controllers
{
    public class ImageController : Controller
    {
        readonly List<HostConfig> _hosts;
        readonly IFileAccessService _fileService;
        readonly IImageService _imageService;
        readonly ILogger _logger;

        public ImageController(IOptions<List<HostConfig>> hosts, IFileAccessService fileServiceService, IImageService imageService, ILogger<ImageController> logger)
        {
            _hosts = hosts.Value;
            _fileService = fileServiceService;
            _imageService = imageService;
            _logger = logger;
        }

        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{id:gridfs}")]
        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{id:gridfs}")]
        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{*id}")]
        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{*id}")]
        public async Task<IActionResult> ImageAsync(string id, string slug, int w, int h, int quality, string options = "")
        {
            return await ImageResult(id, slug, w, h, quality, options);
        }

        [HttpGet("/i/{slug}/{*filepath}")]
        public async Task<IActionResult> ImageFromFilePathAsync(string filepath, string slug)
        {
            return await ImageResult(filepath, slug);
        }

        private async Task<IActionResult> ImageResult(string id, string slug, int w = 0, int h = 0, int quality = 100, string options = "")
        {
            byte[] bytes;
            try
            {
                bytes = await _fileService.GetFileAsync(slug, id, _hosts);
            }
            catch (Exception e)
            {
                _logger.LogError(1000, e.Message);
                throw;
            }

            if (bytes == null)
            {
                _logger.LogError(1000, "File not found");
                return NotFound();
            }

            bytes = _imageService.GetImageAsBytes(w, h, quality, bytes, options, out var mime);
            if (bytes == null)
            {
                _logger.LogError(2000, "File found but image operation failed");
                return StatusCode((int)HttpStatusCode.NotAcceptable);
            }

            return File(bytes, mime);
        }
    }
}

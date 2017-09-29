using System.Collections.Generic;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ImageServer.Core.Controllers
{
    public class ImageController : Controller
    {
        readonly List<HostConfig> _hosts;
        readonly IFileAccessService _fileService;
        readonly IImageService _imageService;

        public ImageController(IOptions<List<HostConfig>> hosts, IFileAccessService fileServiceService, IImageService imageService)
        {
            _hosts = hosts.Value;
            _fileService = fileServiceService;
            _imageService = imageService;
        }

        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{id:gridfs}")]
        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{id:filepath}")]
        [HttpGet("/i/{slug}/{id:filepath}")]
        public async Task<IActionResult> ImageAsync(string id, string slug, int w, int h, int quality)
        {
            return await ImageResult(id, slug, w, h, quality);
        }

        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{id:gridfs}")]
        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{id:filepath}")]
        public async Task<IActionResult> ImageWithOptionsAsync(string id, string slug, int w, int h, int quality, string options = "")
        {
            return await ImageResult(id, slug, w, h, quality, options);
        }

        
        private async Task<IActionResult> ImageResult(string id, string slug, int w, int h, int quality, string options = "")
        {
            var bytes = await _fileService.GetFileAsync(slug, id, _hosts);

            if (bytes == null)
                return NotFound();

            if (w != 0 && h != 0)
                bytes = _imageService.GetImageAsBytes(w, h, quality, bytes, options);

            return File(bytes, "image/jpeg");
        }


    }
}

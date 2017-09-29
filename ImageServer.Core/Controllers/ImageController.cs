using System.Collections.Generic;
using System.Threading.Tasks;
using ImageServer.Core.Helpers;
using ImageServer.Core.Helpers.FileAccess;
using ImageServer.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ImageServer.Core.Controllers
{
    public class ImageController : Controller
    {
        readonly List<HostConfig> _hosts;
        public ImageController(IOptions<List<HostConfig>> hosts)
        {
            _hosts = hosts.Value;
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
            var bytes = await FileAccessHelper.GetFileAsync(slug, id, _hosts);

            if (bytes == null)
                return NotFound();

            if (w != 0 && h != 0)
                bytes = ImageHelper.GetImageAsBytes(w, h, quality, bytes, options);

            return File(bytes, "image/jpeg");
        }


    }
}

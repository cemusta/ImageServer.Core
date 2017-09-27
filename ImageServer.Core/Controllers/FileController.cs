using System.Collections.Generic;
using System.Threading.Tasks;
using ImageServer.Core.Helpers;
using ImageServer.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ImageServer.Core.Controllers
{
    public class FileController : Controller
    {
        readonly List<HostConfig> _hosts;
        public FileController( IOptions<List<HostConfig>> hosts)
        {
            _hosts = hosts.Value;
        }

        [HttpGet("/f/{slug}/{id:gridfs}.{ext}")]
        [HttpGet("/f/{slug}/{id:filepath}")]
        public async Task<IActionResult> FileAsync(string slug, string id)
        {
            var bytes = await FileHelper.GetFileAsync(slug,id, _hosts);

            if (bytes == null)
                return NotFound();

            return File(bytes, "application/octet-stream");
        }
    }
}

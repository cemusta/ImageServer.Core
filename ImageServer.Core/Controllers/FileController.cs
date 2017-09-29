using System.Collections.Generic;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ImageServer.Core.Controllers
{
    public class FileController : Controller
    {
        readonly List<HostConfig> _hosts;
        readonly IFileAccessService _file;

        public FileController( IOptions<List<HostConfig>> hosts, IFileAccessService fileService)
        {
            _hosts = hosts.Value;
            _file = fileService;
        }

        [HttpGet("/f/{slug}/{id:gridfs}.{ext}")]
        [HttpGet("/f/{slug}/{id:filepath}")]
        public async Task<IActionResult> FileAsync(string slug, string id)
        {
            var bytes = await _file.GetFileAsync(slug,id, _hosts);

            if (bytes == null)
                return NotFound();

            return File(bytes, "application/octet-stream");
        }
    }
}

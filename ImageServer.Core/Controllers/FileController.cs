using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageServer.Core.Controllers
{
    public class FileController : Controller
    {
        readonly List<HostConfig> _hosts;
        readonly IFileAccessService _file;
        readonly ILogger _logger;

        public FileController(IOptions<List<HostConfig>> hosts, IFileAccessService fileService, ILogger<FileController> logger)
        {
            _hosts = hosts.Value;
            _file = fileService;
            _logger = logger;
        }

        [HttpGet("/f/{slug}/{id:gridfs}.{ext}")]
        [HttpGet("/f/{slug}/{*id}")]
        public async Task<IActionResult> FileAsync(string slug, string id)
        {
            byte[] bytes;
            try
            {
                bytes = await _file.GetFileAsync(slug, id, _hosts);
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

            return File(bytes, "application/octet-stream");
        }
    }
}

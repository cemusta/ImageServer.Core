using System;
using System.Threading.Tasks;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ImageServer.Core.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileAccessService _file;
        private readonly ILogger<FileController> _logger;

        public FileController(IFileAccessService fileService, ILogger<FileController> logger)
        {
            _file = fileService;
            _logger = logger;
        }

        [HttpGet("/f/{host}/{id:gridfs}.{ext}")]
        [HttpGet("/f/{host}/{*id}")]
        public async Task<IActionResult> FileAsync(string host, string id)
        {
            byte[] bytes;
            try
            {
                bytes = await _file.GetFileAsync(host, id);
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

using System;
using System.Net;
using System.Threading.Tasks;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ImageServer.Core.Controllers
{
    public class ImageController : Controller
    {
        private readonly IFileAccessService _fileService;
        private readonly IImageService _imageService;
        private readonly ILogger<ImageController> _logger;

        public ImageController( IFileAccessService fileServiceService, IImageService imageService, ILogger<ImageController> logger)
        {
            _fileService = fileServiceService;
            _imageService = imageService;
            _logger = logger;
        }

        [HttpGet("/i/{host}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{id:gridfs}")]
        [HttpGet("/i/{host}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{id:gridfs}")]
        [HttpGet("/i/{host}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{*id}")]
        [HttpGet("/i/{host}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{*id}")]
        public async Task<IActionResult> ImageAsync(string id, string host, int w, int h, int quality, string options = "")
        {
            return await ImageResult(id, host, w, h, quality, options);
        }

        [HttpGet("/i/{host}/{*filepath}")]
        public async Task<IActionResult> ImageFromFilePathAsync(string filepath, string host)
        {
            return await ImageResult(filepath, host);
        }

        private async Task<IActionResult> ImageResult(string id, string slug, int w = 0, int h = 0, int quality = 100, string options = "")
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogError("Id is null");
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            byte[] bytes;
            try
            {
                bytes = await _fileService.GetFileAsync(slug, id);
            }
            catch (SlugNotFoundException e)
            {
                _logger.LogError(e.Message);
                return new StatusCodeResult((int) HttpStatusCode.BadRequest);
            }
            catch (GridFsObjectIdException e)
            {
                _logger.LogError("GridFS ObjectId Parse Error:" + e.Message);
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }

            if (bytes == null)
            {
                _logger.LogError("File not found");
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

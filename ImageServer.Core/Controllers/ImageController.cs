using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ImageServer.Core.Controllers
{
    public class ImageController : Controller
    {
        private readonly IFileAccessService _fileService;
        private readonly IFileMetadataService _metadataService;
        private readonly IImageService _imageService;
        private readonly ILogger<ImageController> _logger;

        public ImageController(IFileAccessService fileService, IFileMetadataService metadataService, IImageService imageService, ILogger<ImageController> logger)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _metadataService = metadataService ?? throw new ArgumentNullException(nameof(metadataService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogError("Id is null");
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            if (0 > w || 0 > h)
            {
                _logger.LogError("Width or height is negative");
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            byte[] bytes;
            CustomRatio customRatio = null;
            try
            {
                var host = _fileService.GetHostConfig(slug);

                if (host.WhiteList != null && host.WhiteList.Any() && host.WhiteList.All(x => x != $"{w}x{h}")) //whitelist checking
                {
                    _logger.LogError("Image request cancelled due to whitelist.");
                    return new StatusCodeResult((int)HttpStatusCode.BadRequest);
                }

                bytes = await _fileService.GetFileAsync(slug, id);

                if (bytes == null)
                {
                    _logger.LogError("File not found");
                    return NotFound();
                }
            }
            catch (RedirectToFallbackException e)
            {
                _logger.LogWarning(e, e.Message);
                return Redirect(string.IsNullOrEmpty(options)
                    ? $"/i/{slug}/{quality}/{w}x{h}/{e.FallbackImage}"
                    : $"/i/{slug}/{quality}/{w}x{h}/{options}/{e.FallbackImage}");
            }
            catch (SlugNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            catch (GridFsObjectIdException e)
            {
                _logger.LogError(e, "GridFS ObjectId Parse Error:" + e.Message);
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            catch (TimeoutException e)
            {
                _logger.LogError(e, "Timeout: " + e.Message);
                return new StatusCodeResult((int)HttpStatusCode.GatewayTimeout);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }

            try
            {
                bytes = _imageService.GetImageAsBytes(w, h, quality, bytes, options, out var mime, customRatio);

                if (bytes != null)
                    return File(bytes, mime);

                _logger.LogError("File found but image operation failed by unknown cause");
                return StatusCode((int)HttpStatusCode.NotAcceptable);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Image Service Error: " + e.Message);
                throw;
            }
        }

    }
}

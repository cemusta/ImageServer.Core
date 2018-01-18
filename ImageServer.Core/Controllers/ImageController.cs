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
            _fileService = fileService;
            _metadataService = metadataService;
            _imageService = imageService;
            _logger = logger;
        }

        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{id:gridfs}")]
        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{id:gridfs}")]
        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{hash:metahash}/{id:gridfs}")]
        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{hash:metahash}/{id:gridfs}")]
        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{*id}")]
        [HttpGet("/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{*id}")]
        public async Task<IActionResult> ImageAsync(string id, string slug, int w, int h, int quality, string options = "", string hash = "")
        {
            return await ImageResult(id, slug, w, h, quality, options, hash);
        }

        [HttpGet("/i/{slug}/{*filepath}")]
        public async Task<IActionResult> ImageFromFilePathAsync(string filepath, string slug)
        {
            return await ImageResult(filepath, slug);
        }

        private async Task<IActionResult> ImageResult(string id, string slug, int w = 0, int h = 0, int quality = 100, string options = "", string hash = "")
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogError("Id is null");
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            byte[] bytes;
            CustomRatio ratio = null;
            try
            {
                var host = _fileService.GetHostConfig(slug);

                if (host.WhiteList != null && host.WhiteList.Any() && host.WhiteList.All(x => x != $"{w}x{h}"))
                {
                    _logger.LogError("Image request cancelled due to whitelist.");
                    return new StatusCodeResult((int)HttpStatusCode.BadRequest);
                }

                if (!string.IsNullOrEmpty(hash) && host.Type == HostType.GridFs)
                {
                    hash = hash.Replace("h-", "");
                    var metadata = await _metadataService.GetFileMetadataAsync(host, id);

                    ratio = metadata.CustomRatio.FirstOrDefault(x => x.Hash == hash);

                    if (ratio == null)
                    {
                        _logger.LogError("Image request cancelled due to wrong custom ratio hash");
                        return new StatusCodeResult((int)HttpStatusCode.BadRequest);
                    }

                    if (!(w == 0 | h == 0))
                    {
                        double reqRatio = (double) w / h;
                        if (reqRatio > ratio.MinRatio && reqRatio < ratio.MaxRatio)
                        {
                            _logger.LogError("Image request cancelled due to wrong ratio mismatch");
                            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
                        }
                    }
                }

                bytes = await _fileService.GetFileAsync(slug, id);
            }
            catch (SlugNotFoundException e)
            {
                _logger.LogError(e.Message);
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
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

            bytes = _imageService.GetImageAsBytes(w, h, quality, bytes, options, out var mime, ratio);
            if (bytes == null)
            {
                _logger.LogError(2000, "File found but image operation failed");
                return StatusCode((int)HttpStatusCode.NotAcceptable);
            }

            return File(bytes, mime);
        }
    }
}

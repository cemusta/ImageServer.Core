using System;
using System.Net;
using System.Threading.Tasks;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace ImageServer.Core.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileAccessService _fileService;
        private readonly ILogger<FileController> _logger;

        public FileController(IFileAccessService fileService, ILogger<FileController> logger)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("/f/{slug}/{id:gridfs}.{ext}")]
        [HttpGet("/f/{slug}/{*id}")]
        public async Task<IActionResult> FileAsync(string id, string slug)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            byte[] bytes;

            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogError("Id is null");
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            try
            {
                bytes = await _fileService.GetFileAsync(slug, id);
            }
            catch (SlugNotFoundException e)
            {
                _logger.LogError(e, "Unknown host requested: " + e.Message);
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
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, "Access Denied: " + e.Message);
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }

            if (bytes != null)
            {
                var file = File(bytes, "application/octet-stream");

                using (var sha = System.Security.Cryptography.SHA1.Create())
                {
                    var hash = sha.ComputeHash(bytes);
                    var checksum = $"\"{WebEncoders.Base64UrlEncode(hash)}\"";
                    file.EntityTag = new Microsoft.Net.Http.Headers.EntityTagHeaderValue(checksum);
                }

                return file;
            }

            _logger.LogError("File not found");
            return NotFound();
        }

    }
}

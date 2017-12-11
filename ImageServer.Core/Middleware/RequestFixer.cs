using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ImageServer.Core.Middleware
{
    public class RequestFixer
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestFixer> _logger;

        public RequestFixer(RequestDelegate next, ILogger<RequestFixer> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string url = context.Request.Path.Value;

            if (!string.IsNullOrWhiteSpace(url) && url.Contains("//"))
            {
                _logger.LogWarning($"Bad request, url={url}");
                while (url.Contains("//"))
                {
                    url = url.Replace("//", "/");                    
                }

                context.Request.Path = url;
            }

            await _next.Invoke(context);
        }
    }
}


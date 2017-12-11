using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ImageServer.Core.Middleware
{

    public class PerformanceCounter
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceCounter> _logger;

        public PerformanceCounter(RequestDelegate next, ILogger<PerformanceCounter> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var sw = new Stopwatch();
            sw.Start();
            
            await _next.Invoke(context);


            sw.Stop();
            if (sw.ElapsedMilliseconds > 10000)
                _logger.LogWarning($"Progress took long: {sw.ElapsedMilliseconds}ms");
            else if (sw.ElapsedMilliseconds > 5000)
                _logger.LogWarning($"Progress took long: {sw.ElapsedMilliseconds}ms");
            else if(sw.ElapsedMilliseconds > 1000)
                _logger.LogTrace($"Progress took long: {sw.ElapsedMilliseconds}ms");
        }
    }
}

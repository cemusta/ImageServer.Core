using Microsoft.AspNetCore.Builder;

namespace ImageServer.Core.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestFixer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestFixer>();
        }

        public static IApplicationBuilder UsePerformanceCounter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PerformanceCounter>();
        }
    }
}
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog.Web;

namespace ImageServer.Core
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "conf"));
                    config.AddEnvironmentVariables("IMAGESERVER_");
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder.UseNLog();
                  webBuilder.UseKestrel();
                  webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                  webBuilder.UseStartup<Startup>();
              });

            return hostBuilder;
        }
    }
}

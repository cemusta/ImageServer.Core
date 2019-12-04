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
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();

            var hostBuilder = Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder.UseNLog();
                  webBuilder.UseConfiguration(config);
                  webBuilder.UseKestrel();
                  webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                  webBuilder.UseIISIntegration();
                  webBuilder.UseStartup<Startup>();
              });

            return hostBuilder;
        }
    }
}

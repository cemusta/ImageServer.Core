using System;
using System.IO;
using System.Net.Http;
using ImageServer.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ImageServer.Integration.Tests
{
    public class TestContext : IDisposable
    {
        private TestServer _server;
        private IHost _host;
        public HttpClient Client { get; private set; }

        public TestContext()
        {
            SetUpClient();
        }

        private void SetUpClient()
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "conf"));
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder.UseTestServer();
                  webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                  webBuilder.UseStartup<Startup>();
              });

            _host = hostBuilder.Build();
            _host.StartAsync();

            Client = _host.GetTestClient();
        }

        public void Dispose()
        {
            _host?.Dispose();
            Client?.Dispose();
        }
    }
}
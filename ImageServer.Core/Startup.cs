using System.Collections.Generic;
using ImageServer.Core.Middleware;
using ImageServer.Core.Model;
using ImageServer.Core.Route;
using ImageServer.Core.Services;
using ImageServer.Core.Services.FileAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ImageServer.Core
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        private ILogger _logger;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath($"{env.ContentRootPath}/conf")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets("local");
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //call this in case you need aspnet-user-authtype/aspnet-user-identity - nlog using it
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add framework services.
            services.AddRazorPages();

            services.AddMemoryCache();

            var hosts = Configuration.GetSection("Hosts");
            services.Configure<List<HostConfig>>(hosts);

            var strategyDictionary = new Dictionary<HostType, IFileAccessStrategy>
            {
                {HostType.GridFs, new GridFsAccess()},
                {HostType.FileSystem, new FileSystemAccess()},
                {HostType.Web, new WebAccess()},
                {HostType.GoogleBucket, new GoogleStorageBucketAccess() }
            };
            services.AddSingleton<IDictionary<HostType, IFileAccessStrategy>>(strategyDictionary);

            services.AddTransient<IFileAccessService, FileAccessService>();
            services.AddTransient<IImageService, ImageService>();

            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("gridfs", typeof(GridFsRouteConstraint)));
            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("opt", typeof(OptionsRouteConstraint)));
            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("filepath", typeof(FilePathRouteConstraint)));
            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("metahash", typeof(MetaHashRouteConstraint)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime, ILoggerFactory loggerFactory)
        {
            // passing kibana address to nlog
            LogManager.Configuration.Variables["tcpAddress"] = $"{Configuration["Logging:tcpAddress"]}";

            app.UseRouting();

            app.UsePerformanceCounter();

            app.UseRequestFixer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);

            _logger = loggerFactory.CreateLogger("StartupLogger");
        }

        private void OnStarted()
        {
            _logger.LogInformation("Application started...");
        }
        private void OnStopping()
        {
            _logger.LogInformation("Application stopping...");
        }

        private void OnStopped()
        {
            _logger.LogInformation("Application stopped...");
        }
    }
}

using System.Collections.Generic;
using ImageServer.Core.Middleware;
using ImageServer.Core.Model;
using ImageServer.Core.Route;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ImageServer.Core
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        private ILogger _logger;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets("local");
            }

            env.ConfigureNLog("nlog.config");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //call this in case you need aspnet-user-authtype/aspnet-user-identity - nlog kullanıyor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add framework services.
            services.AddMvc();

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddMemoryCache();

            var hosts = Configuration.GetSection("Hosts");
            services.Configure<List<HostConfig>>(hosts);

            services.AddTransient<IFileAccessService, FileAccessService>();
            services.AddTransient<IImageService, ImageService>();

            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("gridfs", typeof(GridFsRouteConstraint)));
            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("opt", typeof(OptionsRouteConstraint)));
            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("filepath", typeof(FilePathRouteConstraint)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime, ILoggerFactory loggerFactory)
        {
            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();

            //add NLog.Web
            app.AddNLogWeb();

            LogManager.Configuration.Variables["tcpAddress"] = $"{Configuration["Logging:tcpAddress"]}";

            app.UsePerformanceCounter();

            app.UseRequestFixer();            

            app.UseMvc();
            
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

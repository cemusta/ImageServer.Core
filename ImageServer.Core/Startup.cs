using System.Collections.Generic;
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
using NLog.Extensions.Logging;
using NLog.Web;

namespace ImageServer.Core
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        
        public Startup(IHostingEnvironment env, ILogger<Startup> logger)
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


            logger.LogInformation("Application started...");
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //call this in case you need aspnet-user-authtype/aspnet-user-identity - nlog kullanıyor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add framework services.
            services.AddMvc();

            services.AddSingleton<IConfiguration>(Configuration);

            var hosts = Configuration.GetSection("Hosts");
            services.Configure<List<HostConfig>>(hosts);

            services.AddTransient<IFileAccessService,FileAccessService>();
            services.AddTransient<IImageService, ImageService>();

            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("gridfs", typeof(GridFsRouteConstraint)));
            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("opt", typeof(OptionsRouteConstraint)));
            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("filepath", typeof(FilePathRouteConstraint)));
            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("slug", typeof(SlugRouteConstraint)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc();

            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();

            //add NLog.Web
            app.AddNLogWeb();

        }
    }
}

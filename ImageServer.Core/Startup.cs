using System.Collections.Generic;
using ImageServer.Core.Model;
using ImageServer.Core.Route;
using ImageServer.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ImageServer.Core
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        
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
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();           

            app.UseMvc();

        }
    }
}

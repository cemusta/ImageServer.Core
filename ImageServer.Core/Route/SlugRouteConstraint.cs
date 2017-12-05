using System.Collections.Generic;
using System.Linq;
using ImageServer.Core.Controllers;
using ImageServer.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageServer.Core.Route
{
    public class SlugRouteConstraint : IRouteConstraint
    {
        private readonly List<HostConfig> _hosts;
        private readonly ILogger<FileController> _logger;

        public SlugRouteConstraint(IOptions<List<HostConfig>> hosts,  ILogger<FileController> logger)
        {
            _hosts = hosts.Value;
            _logger = logger;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string val = values[routeKey] as string;
            if (string.IsNullOrWhiteSpace(val))
                return false;

            var slugfound = _hosts.Any(x => x.Slug == val);


            if (slugfound)
                return true;

            _logger.LogWarning($"Unknown slug requested: {val}");
            return false;
        }

    }
}
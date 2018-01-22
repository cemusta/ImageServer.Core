using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ImageServer.Core.Route
{
    public class FilePathRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            var val = values[routeKey] as string;
            if (string.IsNullOrWhiteSpace(val)) 
                return false;

            var regex = new Regex(@"(.+)");
            var match = regex.Match(val);
            return match.Success;
        }
    }
}
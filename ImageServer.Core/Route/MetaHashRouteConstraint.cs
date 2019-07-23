using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ImageServer.Core.Route
{
    public class MetaHashRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            var val = values[routeKey] as string;
            if (string.IsNullOrWhiteSpace(val))
                return false;
            if (val.Length != 32)
                return false;

            var regex = new Regex(@"[a-z0-9]{32}$");
            var match = regex.Match(val);
            return match.Success;
        }
    }
}
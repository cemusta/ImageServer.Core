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
            string val = values[routeKey] as string;
            if (string.IsNullOrWhiteSpace(val)) 
                return false;

            Regex regex = new Regex(@"h-[a-z0-9]{32}$");
            Match match = regex.Match(val);
            return match.Success;
        }
    }
}
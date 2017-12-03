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
            string val = values[routeKey] as string;
            if (string.IsNullOrWhiteSpace(val)) 
                return false;

            Regex regex = new Regex(@"(.+)");
            Match match = regex.Match(val);
            if (match.Success)
            {
                return true;
            }

            return false;
        }
    }
}
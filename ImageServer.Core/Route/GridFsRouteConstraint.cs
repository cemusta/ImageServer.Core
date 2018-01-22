using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ImageServer.Core.Route
{
    public class GridFsRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            var val = values[routeKey] as string;
            if (string.IsNullOrWhiteSpace(val) || val.Length != 24) //gridfs id is 24 char.
                return false;

            var regex = new Regex(@"([0-9a-fA-F]{24})");
            var match = regex.Match(val);
            return match.Success;
        }

    }
}
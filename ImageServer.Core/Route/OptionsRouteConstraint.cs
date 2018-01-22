using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ImageServer.Core.Route
{
    public class OptionsRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var val = values[routeKey] as string;
            if (string.IsNullOrWhiteSpace(val))
                return false;

            var regex = new Regex(@"([tgf]{1,3})");
            var match = regex.Match(val);
            return match.Success
                   && match.Value.Length == val.Length
                   && StringHasUniqueChars(match.Value);
        }

        bool StringHasUniqueChars(string key)
        {
            var charTable = string.Empty;

            foreach (var character in key)
            {
                if (charTable.IndexOf(character) == -1)
                {
                    charTable += character;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
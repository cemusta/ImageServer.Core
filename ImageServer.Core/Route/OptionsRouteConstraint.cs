using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ImageServer.Core.Route
{
    public class OptionsRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string val = values[routeKey] as string;
            if (string.IsNullOrWhiteSpace(val))
                return false;

            Regex regex = new Regex(@"([tgf]{1,3})");
            Match match = regex.Match(val);
            if (match.Success
                && match.Value.Length == val.Length
                && StringHasUniqueChars(match.Value))
            {
                return true;
            }

            return false;
        }

        bool StringHasUniqueChars(string key)
        {
            string table = string.Empty;

            foreach (char value in key)
            {
                if (table.IndexOf(value) == -1)
                {
                    table += value;
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
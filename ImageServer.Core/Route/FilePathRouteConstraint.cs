using Microsoft.AspNetCore.Routing.Constraints;

namespace ImageServer.Core.Route
{
    public class FilePathRouteConstraint : RegexRouteConstraint
    {
        public FilePathRouteConstraint() : base(@"(.*)")
        {
        }
    }
}
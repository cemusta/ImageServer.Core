using Microsoft.AspNetCore.Routing.Constraints;

namespace ImageServer.Core.Route
{
    public class GridFsRouteConstraint : RegexRouteConstraint
    {
        public GridFsRouteConstraint() : base(@"([0-9a-fA-F]{24})")
        {
        }
    }
}
using Microsoft.AspNetCore.Routing.Constraints;

namespace ImageServer.Core.Route
{
    public class OptionsRouteConstraint : RegexRouteConstraint
    {
        public OptionsRouteConstraint() : base(@"([tgf]{1,3})?")
        {
        }
    }
}
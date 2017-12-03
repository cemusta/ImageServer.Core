using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ImageServer.Integration.Tests
{
    [Collection("SystemCollection")]
    public class PingTests
    {
        public readonly TestContext Context;

        public PingTests(TestContext context)
        {
            Context = context;
        }

        [Fact]
        public async Task StatusReturnsOkResponse()
        {
            var response = await Context.Client.GetAsync("/status");

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task VersionReturnsOkResponse()
        {
            var response = await Context.Client.GetAsync("/ver");

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
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

        [Theory]
        [InlineData("/status", HttpStatusCode.OK)]
        [InlineData("/ver", HttpStatusCode.OK)]
        [InlineData("/version", HttpStatusCode.OK)]
        [InlineData("/formats", HttpStatusCode.OK)]
        [InlineData("/hosts", HttpStatusCode.OK)]
        [InlineData("/unkown", HttpStatusCode.NotFound)]
        public async Task StatusReturnsOkResponse(string url, HttpStatusCode code)
        {
            var response = await Context.Client.GetAsync(url);            

            response.StatusCode.Should().Be(code);
        }

    }
}
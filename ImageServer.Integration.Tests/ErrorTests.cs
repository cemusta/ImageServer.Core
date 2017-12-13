using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ImageServer.Integration.Tests
{
    [Collection("SystemCollection")]
    public class ErrorTests
    {
        public readonly TestContext Context;

        public ErrorTests(TestContext context)
        {
            Context = context;
        }

        [Theory]
        [InlineData("/i/unkown/testimage", HttpStatusCode.BadRequest)]
        [InlineData("/f/unkown/testimage", HttpStatusCode.BadRequest)]
        [InlineData("/i/hurriyet/testimage", HttpStatusCode.BadRequest)]
        [InlineData("/f/hurriyet/testimage", HttpStatusCode.BadRequest)]
        public async Task UnknownHost(string url, HttpStatusCode code)
        {
            var response = await Context.Client.GetAsync(url);            

            response.StatusCode.Should().Be(code);
        }
    }
}
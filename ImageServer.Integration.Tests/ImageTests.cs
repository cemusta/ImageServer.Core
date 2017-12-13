using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ImageServer.Integration.Tests
{
    [Collection("SystemCollection")]
    public class ImageTests
    {
        public readonly TestContext Context;

        public ImageTests(TestContext context)
        {
            Context = context;
        }

        [Theory]
        [InlineData("/i/proxy/v2/1423x949q90/911/zPCsEi.jpg", HttpStatusCode.OK)]
        [InlineData("/i/proxy/75/0x0/v2/1423x949q90/911/zPCsEi.jpg", HttpStatusCode.OK)]
        [InlineData("/i/proxy/75/0x0/g/v2/1423x949q90/911/zPCsEi.jpg", HttpStatusCode.OK)]
        [InlineData("/i/proxy/75/0x0/f/v2/1423x949q90/911/zPCsEi.jpg", HttpStatusCode.OK)]
        [InlineData("/i/proxy/75/4x4/v2/1423x949q90/911/zPCsEi.jpg", HttpStatusCode.OK)]
        [InlineData("/i/proxy/75/0x4/v2/1423x949q90/911/zPCsEi.jpg", HttpStatusCode.OK)]
        public async Task ProxyTest(string url, HttpStatusCode code)
        {
            var response = await Context.Client.GetAsync(url);

            response.StatusCode.Should().Be(code);
        }

    }
}
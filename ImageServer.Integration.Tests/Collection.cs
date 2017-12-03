using Xunit;

namespace ImageServer.Integration.Tests
{
    [CollectionDefinition("SystemCollection")]
    public class Collection : ICollectionFixture<TestContext>
    {

    }
}
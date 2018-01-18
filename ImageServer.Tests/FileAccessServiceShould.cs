using System;
using System.Collections.Generic;
using ImageServer.Core.Model;
using ImageServer.Core.Services;
using Microsoft.Extensions.Options;
using Xunit;
using Moq;

namespace ImageServer.Tests
{
    public class FileAccessServiceShould
    {
        [Fact]
        public void GiveNullExceptionWhenHostListNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FileAccessService(null));
        }

        [Fact]
        public void GiveSlugNotFoundExceptionWhenHostNotFound()
        {
            var mock = new Mock<IOptions<List<HostConfig>>>();
            mock.Setup(x => x.Value).Returns(new List<HostConfig>
            {
                new HostConfig
                {
                    Slug = "gridfs",
                    Type = HostType.GridFs,
                    DatabaseName = "someDb",
                    ConnectionString = "someConnStr"
                }
            });

            var sut = new FileAccessService(mock.Object);

            Assert.Throws<SlugNotFoundException>(() => sut.GetHostConfig("otherSlug"));
        }

    }
}
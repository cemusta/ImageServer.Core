using System;
using System.Collections.Generic;
using ImageServer.Core.Model;
using ImageServer.Core.Services;
using ImageServer.Core.Services.FileAccess;
using Microsoft.Extensions.Options;
using Xunit;
using AutoFixture;
using AutoFixture.AutoMoq;

namespace ImageServer.Tests
{
    public class FileAccessServiceShould
    {
        private readonly IFixture _fixture;

        public FileAccessServiceShould()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }
        [Fact]
        public void GiveNullExceptionWhenHostListNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FileAccessService(null, null));
        }

        [Fact]
        public void GiveSlugNotFoundExceptionWhenHostNotFound()
        {
            //var mock = new Mock<IOptions<List<HostConfig>>>();
            //mock.Setup(x => x.Value).Returns(new List<HostConfig>
            //{
            //    new HostConfig
            //    {
            //        Slug = "gridfs",
            //        Type = HostType.GridFs,
            //        DatabaseName = "someDb",
            //        ConnectionString = "someConnStr"
            //    }
            //});


            var sut = new FileAccessService(_fixture.Create<IOptions<List<HostConfig>>>(), _fixture.Create<IDictionary<HostType, IFileAccessStrategy>>());

            Assert.Throws<SlugNotFoundException>(() => sut.GetHostConfig("otherSlug"));
        }

    }
}
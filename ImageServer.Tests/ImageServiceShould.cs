using System.Linq;
using ImageServer.Core.Services;
using Xunit;

namespace ImageServer.Tests
{
    public class ImageServiceShould
    {
        [Fact]
        public void ReturnsVersion()
        {
            var sut = new ImageService();
            
            var version = sut.GetVersion();
            
            Assert.True(!string.IsNullOrEmpty(version));
        }

        [Fact]
        public void ReturnsFeatures()
        {
            var sut = new ImageService();

            var version = sut.GetFeatures();
            
            Assert.True(!string.IsNullOrEmpty(version));
        }

        [Fact]
        public void ReturnsSupportedFormats()
        {
            var sut = new ImageService();
            
            var formats = sut.GetSupportedFormats();
            
            Assert.True(formats.Any());
        }
    }
}

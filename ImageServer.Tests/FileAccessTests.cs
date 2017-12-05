//using System;
//using ImageServer.Core.Model;
//using ImageServer.Core.Services;
//using ImageServer.Core.Services.FileAccess;
//using Xunit;

//namespace ImageServer.Tests
//{
//    public class FileAccessTests
//    {
//        [Theory]
//        [InlineData(HostType.FileSystem, typeof(FileSystemAccess))]
//        [InlineData(HostType.GridFs, typeof(GridFsAccess))]
//        [InlineData(HostType.Web, typeof(WebAccess))]
//        public void GetAccess_Returns_Value_For_Enums(HostType parameterValue, Type expected)
//        {
//            // Arrange
//            var constraint = new FileAccessService(null);

//            // Act
//            var actual = constraint.GetAccess(parameterValue);

//            // Assert
//            Assert.Equal(expected, actual.GetType());
//        }

//        [Theory]
//        [InlineData(4,"4")]
//        [InlineData(5,"5")]
//        public void GetAccess_Throws_Exception_When_Null(HostType parameterValue, string message)
//        {
//            // Arrange
//            var constraint = new FileAccessService(null);

//            // Act
//            Exception ex = Assert.Throws<NotImplementedException>(() => constraint.GetAccess(parameterValue));

//            // Assert
//            Assert.Equal(message, ex.Message);
//        }

//    }
//}
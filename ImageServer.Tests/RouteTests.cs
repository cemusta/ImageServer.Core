using ImageServer.Core.Route;
using ImageServer.Tests.Helper;
using Xunit;

namespace ImageServer.Tests
{
    public class RouteTests
    {
        [Theory]
        [InlineData("55ea0a2df018fbb8f8660eab", true)]
        [InlineData("zzea0a2df018fbb8f8660eab", false)]
        [InlineData("zzea0a2df018fbb8f8660e", false)]
        [InlineData("zzea0a2df018fbb8f8660eabaa", true)]
        public void Test_GridFsRouteConstraint(string parameterValue, bool expected)
        {
            // Arrange
            var constraint = new GridFsRouteConstraint();

            // Act
            var actual = ConstraintsTestHelper.TestConstraint(constraint, parameterValue);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("55ea0a2df018fbb8f8660eab", true)]
        [InlineData("zzea0a2df018fbb8f8660eab", false)]
        [InlineData("zzea0a2df018fbb8f8660e", false)]
        [InlineData("/2017-10-9/eaccf175-33bf-4317-9e6c-13f2b7325dc6.jpg", true)]
        public void Test_FilePathRouteConstraint(string parameterValue, bool expected)
        {
            // Arrange
            var constraint = new FilePathRouteConstraint();

            // Act
            var actual = ConstraintsTestHelper.TestConstraint(constraint, parameterValue);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ftg", true)]
        [InlineData("zg", false)]
        [InlineData("a2", false)]
        [InlineData("ftggg", false)]
        public void Test_OptionsRouteConstraint(string parameterValue, bool expected)
        {
            // Arrange
            var constraint = new OptionsRouteConstraint();

            // Act
            var actual = ConstraintsTestHelper.TestConstraint(constraint, parameterValue);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}

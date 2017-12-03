using ImageServer.Core.Route;
using ImageServer.Tests.Helper;
using Xunit;

namespace ImageServer.Tests
{
    public class RouteTests
    {
        [Theory]
        [InlineData("55ea0a2df018fbb8f8660eab", true)]
        [InlineData("55ea0a2df018fbb8f8660eab0", false)]    //+1 char
        [InlineData("55ea0a2df018fbb8f8660ea", false)]      //-1 char 
        [InlineData("zzea0a2df018fbb8f8660eab", false)]     //illegal char (z)
        [InlineData("zzea0a2df018fbb8f8660e", false)]       //illegal char (z) -1 char
        [InlineData("zzea0a2df018fbb8f8660eabaa", false)]   //illegal char (z) but rest is ok.
        [InlineData(" ", false)]
        [InlineData(null, false)]
        public void Test_GridFsRouteConstraint(string parameterValue, bool expected)
        {
            // Arrange
            var constraint = new GridFsRouteConstraint();

            // Act
            var actual = ConstraintsTestHelper.TestConstraint(constraint, parameterValue);

            // Assert
            Assert.Equal(expected, actual);
        }

        //[Theory]
        //[InlineData("55ea0a2df018fbb8f8660eab", true)]
        //[InlineData("zzea0a2df018fbb8f8660eab", false)]
        //[InlineData("zzea0a2df018fbb8f8660e", false)]
        //[InlineData("/2017-10-9/eaccf175-33bf-4317-9e6c-13f2b7325dc6.jpg", true)]
        //[InlineData(" ", false)]
        //[InlineData(null, false)]
        //public void Test_FilePathRouteConstraint(string parameterValue, bool expected)
        //{
        //    // Arrange
        //    var constraint = new FilePathRouteConstraint();

        //    // Act
        //    var actual = ConstraintsTestHelper.TestConstraint(constraint, parameterValue);

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        [Theory]
        [InlineData("ftg", true)]
        [InlineData("gtf", true)]
        [InlineData("gf", true)]
        [InlineData("zg", false)]       //false char among options
        [InlineData("a2", false)]       //all false
        [InlineData("ftggg", false)]    //more than 3
        [InlineData("ff", false)]       //duplicate
        [InlineData("ffg", false)]      //duplicate
        [InlineData(" ", false)]
        [InlineData(null, false)]
        public void Test_OptionsRouteConstraint(string parameterValue, bool expected)
        {
            // Arrange
            var constraint = new OptionsRouteConstraint();

            // Act
            var actual = ConstraintsTestHelper.TestConstraint(constraint, parameterValue);

            // Assert
            Assert.Equal(expected, actual);
        }

        //   /i/hurriyettest/75/900x0/59ca30e46d7745217caa2511.jpg 

    }
}

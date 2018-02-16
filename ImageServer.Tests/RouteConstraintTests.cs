using ImageServer.Core.Route;
using ImageServer.Tests.Helper;
using Xunit;

namespace ImageServer.Tests
{
    public class RouteConstraintTests
    {
        [Theory]
        [InlineData("55ea0a2df018fbb8f8660eab", true)]
        [InlineData("55ea0a2df018fbb8f8660eab.jpg", true)]
        [InlineData("55ea0a2df018fbb8f8660eab.gif", true)]
        [InlineData("55ea0a2df018fbb8f8660eab.png", true)]
        [InlineData("55ea0a2df018fbb8f8660eab.jpg.jpg", false)]
        [InlineData("55ea0a2df018fbb8f8660eab.", true)]
        [InlineData("55ea0a2df018fbb8f8660eab0", false)]    //+1 char
        [InlineData("55ea0a2df018fbb8f8660ea", false)]      //-1 char 
        [InlineData("zzea0a2df018fbb8f8660eab", false)]     //illegal char (z)
        [InlineData("zzea0a2df018fbb8f8660e", false)]       //illegal char (z) -1 char
        [InlineData("zzea0a2df018fbb8f8660eabaa", false)]   //illegal char (z) but rest is ok.
        [InlineData(" ", false)]
        [InlineData(null, false)]
        public void Test_GridFsRouteConstraint(string parameterValue, bool expected)
        {
            var constraint = new GridFsRouteConstraint();

            var actual = ConstraintsTestHelper.TestConstraint(constraint, parameterValue);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("1234567890a1234567890a1234567890", true)]
        [InlineData("1234567890a1234567890a12345678902", false)]    //+1 char
        [InlineData("1234567890a1234567890a123456789", false)]      //-1 char 
        [InlineData("1234567890a1234567890a123456789+", false)]     //illegal char (+)
        [InlineData(" ", false)]
        [InlineData(null, false)]
        public void Test_MetaHashRouteConstraint(string parameterValue, bool expected)
        {
            var constraint = new MetaHashRouteConstraint();

            var actual = ConstraintsTestHelper.TestConstraint(constraint, parameterValue);

            Assert.Equal(expected, actual);
        }

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
            var constraint = new OptionsRouteConstraint();

            var actual = ConstraintsTestHelper.TestConstraint(constraint, parameterValue);

            Assert.Equal(expected, actual);
        }
    }
}

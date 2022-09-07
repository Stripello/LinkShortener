using LinkShortener.Pl;
using System;
using Xunit;


namespace BLUnitTests
{
    public class ShortLinkConverterTests
    {
        #region int to link conversion
        [Theory]
        [InlineData("A", 0)]
        [InlineData("B", 1)]
        [InlineData("BA", 26)]
        [InlineData("CAA", 1352)]
        [InlineData("CAD", 1355)]
        [InlineData("DA", 78)]
        [InlineData("DC", 80)]
        [InlineData("CA", 52)]
        public void IntToLink_ValidData_Succeed(string expected, int incomingInt)
        {
            var shortLinkConverter = new ShortLinkConverter();
            var acrual = shortLinkConverter.IntToLink(incomingInt);
            Assert.Equal(expected, acrual);
        }

        [Fact]
        public void IntToLink_NegativeInt_Fail()
        {
            //Arrange
            var incomingInt = -1;
            var shortLinkConverter = new ShortLinkConverter();

            //Assert
            Assert.Throws<ArgumentException>(() => shortLinkConverter.IntToLink(incomingInt));
        }
        #endregion

        #region link to int conversion
        [Theory]
        [InlineData("A", 0)]
        [InlineData("B", 1)]
        [InlineData("BA", 26)]
        [InlineData("CAA", 1352)]
        [InlineData("CAD", 1355)]
        [InlineData("DA", 78)]
        [InlineData("DC", 80)]
        [InlineData("CA", 52)]
        public void LinkToInt_ValidData_Succeed(string incomingString, int expected)
        {
            var shortLinkConverter = new ShortLinkConverter();
            var acrual = shortLinkConverter.LinkToInt(incomingString);
            Assert.Equal(expected, acrual);
        }

        [Fact]
        public void LinkToInt_TooLongLink_Fail()
        {
            //Arrange
            string stringToInsert = "WAAAAAA";
            var shortLinkConverter = new ShortLinkConverter();

            //Assert
            Assert.Throws<ArgumentException>(() => shortLinkConverter.LinkToInt(stringToInsert));
        }

        [Theory]
        [InlineData("AC")]
        [InlineData("AD")]
        [InlineData("ABC")]
        [InlineData("ABCD")]
        public void LinkToInt_IncorrectFirstChar_Fail(string incomingLink)
        {
            var shortLinkConverter = new ShortLinkConverter();
            Assert.Throws<ArgumentException>(() => shortLinkConverter.LinkToInt(incomingLink));
        }

        [Theory]
        [InlineData("HUv")]
        [InlineData("C#")]
        [InlineData("\\")]
        [InlineData("\"")]
        [InlineData("&")]
        [InlineData("?")]
        [InlineData("15")]
        public void LinkToInt_IncorrectCharInLink_Fail(string incomigLink)
        {
            var shortLinkConverter = new ShortLinkConverter();
            Assert.Throws<ArgumentException>(() => shortLinkConverter.LinkToInt(incomigLink));
        }

        [Fact]
        public void LinkToInt_Null_Fail()
        {
            var shortLinkConverter = new ShortLinkConverter();
            Assert.Throws<ArgumentException>(() => shortLinkConverter.LinkToInt(null));
        }
        #endregion
    }
}
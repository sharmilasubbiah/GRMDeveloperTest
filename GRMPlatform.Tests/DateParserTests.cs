using System;
using Xunit;
using GRMPlatform.Utilities;

namespace GRMPlatform.Tests
{
    public class DateParserTests
    {
        [Theory]
        [InlineData("1st Feb 2012", 2012, 2, 1)]
        [InlineData("25st Dec 2012", 2012, 12, 25)]
        [InlineData("31st Dec 2012", 2012, 12, 31)]
        [InlineData("1st Mar 2011", 2011, 3, 1)]
        [InlineData("1st June 2012", 2012, 6, 1)]
        [InlineData("1st Aug 2012", 2012, 8, 1)]
        [InlineData("27th Dec 2012", 2012, 12, 27)]
        [InlineData("1st April 2012", 2012, 4, 1)]
        [InlineData("1st March 2012", 2012, 3, 1)]
        public void TestDateParsing(string input, int expectedYear, int expectedMonth, int expectedDay)
        {
            // Act
            DateTime result = DateParser.Parse(input);

            // Assert
            Assert.Equal(expectedYear, result.Year);
            Assert.Equal(expectedMonth, result.Month);
            Assert.Equal(expectedDay, result.Day);
        }
        
[Theory]
[InlineData("Invalid Date")]
[InlineData("32nd Feb 2012")]        // Invalid day
[InlineData("March 1st 2012")]       // Wrong order
[InlineData("2012-03-01")]           // Wrong format
[InlineData("")]                      // Empty string
[InlineData("1st 2012")]             // Missing month
public void TestInvalidDateFormats_ShouldThrowFormatException(string invalidDate)
{
    // Act & Assert
    Assert.Throws<FormatException>(() => DateParser.Parse(invalidDate));
}
    }
}
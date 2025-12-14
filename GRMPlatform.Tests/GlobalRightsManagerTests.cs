using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GRMPlatform.Models;
using GRMPlatform.Services;

namespace GRMPlatform.Tests
{
    public class GlobalRightsManagerTests
    {
        private List<MusicContract> GetTestMusicContracts()
        {
            return new List<MusicContract>
            {
                new MusicContract
                {
                    Artist = "Tinie Tempah",
                    Title = "Frisky (Live from SoHo)",
                    Usages = new List<string> { "digital download", "streaming" },
                    StartDate = "1st Feb 2012",
                    EndDate = null
                },
                new MusicContract
                {
                    Artist = "Tinie Tempah",
                    Title = "Miami 2 Ibiza",
                    Usages = new List<string> { "digital download" },
                    StartDate = "1st Feb 2012",
                    EndDate = null
                },
                new MusicContract
                {
                    Artist = "Tinie Tempah",
                    Title = "Till I'm Gone",
                    Usages = new List<string> { "digital download" },
                    StartDate = "1st Aug 2012",
                    EndDate = null
                },
                new MusicContract
                {
                    Artist = "Monkey Claw",
                    Title = "Black Mountain",
                    Usages = new List<string> { "digital download" },
                    StartDate = "1st Feb 2012",
                    EndDate = null
                },
                new MusicContract
                {
                    Artist = "Monkey Claw",
                    Title = "Iron Horse",
                    Usages = new List<string> { "digital download", "streaming" },
                    StartDate = "1st June 2012",
                    EndDate = null
                },
                new MusicContract
                {
                    Artist = "Monkey Claw",
                    Title = "Motor Mouth",
                    Usages = new List<string> { "digital download", "streaming" },
                    StartDate = "1st Mar 2011",
                    EndDate = null
                },
                new MusicContract
                {
                    Artist = "Monkey Claw",
                    Title = "Christmas Special",
                    Usages = new List<string> { "streaming" },
                    StartDate = "25st Dec 2012",
                    EndDate = "31st Dec 2012"
                }
            };
        }

        private List<PartnerContract> GetTestPartnerContracts()
        {
            return new List<PartnerContract>
            {
                new PartnerContract { Partner = "ITunes", Usage = "digital download" },
                new PartnerContract { Partner = "YouTube", Usage = "streaming" }
            };
        }

        [Fact]
        public void TestScenario1_ITunes_1stMarch2012()
        {
            // Arrange
            var grm = new GlobalRightsManager(GetTestMusicContracts(), GetTestPartnerContracts());

            // Act
            var results = grm.GetAvailableProducts("ITunes", "1st March 2012");

            // Assert
            Assert.Equal(4, results.Count);
            
            var orderedResults = results.OrderBy(r => r.Artist).ThenBy(r => r.Title).ToList();
            
            Assert.Equal("Monkey Claw", orderedResults[0].Artist);
            Assert.Equal("Black Mountain", orderedResults[0].Title);
            Assert.Equal("digital download", orderedResults[0].Usage);
            
            Assert.Equal("Monkey Claw", orderedResults[1].Artist);
            Assert.Equal("Motor Mouth", orderedResults[1].Title);
            Assert.Equal("digital download", orderedResults[1].Usage);
            
            Assert.Equal("Tinie Tempah", orderedResults[2].Artist);
            Assert.Equal("Frisky (Live from SoHo)", orderedResults[2].Title);
            Assert.Equal("digital download", orderedResults[2].Usage);
            
            Assert.Equal("Tinie Tempah", orderedResults[3].Artist);
            Assert.Equal("Miami 2 Ibiza", orderedResults[3].Title);
            Assert.Equal("digital download", orderedResults[3].Usage);
        }

        [Fact]
        public void TestScenario2_YouTube_1stApril2012()
        {
            // Arrange
            var grm = new GlobalRightsManager(GetTestMusicContracts(), GetTestPartnerContracts());

            // Act
            var results = grm.GetAvailableProducts("YouTube", "1st April 2012");

            // Assert
            Assert.Equal(2, results.Count);
            
            var orderedResults = results.OrderBy(r => r.Artist).ThenBy(r => r.Title).ToList();
            
            Assert.Equal("Monkey Claw", orderedResults[0].Artist);
            Assert.Equal("Motor Mouth", orderedResults[0].Title);
            Assert.Equal("streaming", orderedResults[0].Usage);
            
            Assert.Equal("Tinie Tempah", orderedResults[1].Artist);
            Assert.Equal("Frisky (Live from SoHo)", orderedResults[1].Title);
            Assert.Equal("streaming", orderedResults[1].Usage);
        }

        [Fact]
        public void TestScenario3_YouTube_27thDec2012()
        {
            // Arrange
            var grm = new GlobalRightsManager(GetTestMusicContracts(), GetTestPartnerContracts());

            // Act
            var results = grm.GetAvailableProducts("YouTube", "27th Dec 2012");

            // Assert
            Assert.Equal(4, results.Count);
            
            var orderedResults = results.OrderBy(r => r.Artist).ThenBy(r => r.Title).ToList();
            
            Assert.Equal("Monkey Claw", orderedResults[0].Artist);
            Assert.Equal("Christmas Special", orderedResults[0].Title);
            Assert.Equal("streaming", orderedResults[0].Usage);
            Assert.Equal("25st Dec 2012", orderedResults[0].StartDate);
            Assert.Equal("31st Dec 2012", orderedResults[0].EndDate);
            
            Assert.Equal("Monkey Claw", orderedResults[1].Artist);
            Assert.Equal("Iron Horse", orderedResults[1].Title);
            Assert.Equal("streaming", orderedResults[1].Usage);
            
            Assert.Equal("Monkey Claw", orderedResults[2].Artist);
            Assert.Equal("Motor Mouth", orderedResults[2].Title);
            Assert.Equal("streaming", orderedResults[2].Usage);
            
            Assert.Equal("Tinie Tempah", orderedResults[3].Artist);
            Assert.Equal("Frisky (Live from SoHo)", orderedResults[3].Title);
            Assert.Equal("streaming", orderedResults[3].Usage);
        }

        [Fact]
        public void TestDateNotYetStarted()
        {
            // Arrange
            var grm = new GlobalRightsManager(GetTestMusicContracts(), GetTestPartnerContracts());

            // Act - Query before any contracts start
            var results = grm.GetAvailableProducts("ITunes", "1st Jan 2011");

            // Assert - Should be empty
            Assert.Empty(results);
        }

        [Fact]
        public void TestDateAfterEndDate()
        {
            // Arrange
            var grm = new GlobalRightsManager(GetTestMusicContracts(), GetTestPartnerContracts());

            // Act - Query after Christmas Special ends
            var results = grm.GetAvailableProducts("YouTube", "1st Jan 2013");

            // Assert - Christmas Special should not be included
            Assert.DoesNotContain(results, r => r.Title == "Christmas Special");
            Assert.Equal(3, results.Count); // Iron Horse, Motor Mouth, Frisky
        }

        [Fact]
        public void TestInvalidPartner()
        {
            // Arrange
            var grm = new GlobalRightsManager(GetTestMusicContracts(), GetTestPartnerContracts());

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                grm.GetAvailableProducts("Spotify", "1st March 2012"));
        }

        [Fact]
        public void TestCaseInsensitivePartnerName()
        {
            // Arrange
            var grm = new GlobalRightsManager(GetTestMusicContracts(), GetTestPartnerContracts());

            // Act - Test with different cases
            var results1 = grm.GetAvailableProducts("ITUNES", "1st March 2012");
            var results2 = grm.GetAvailableProducts("itunes", "1st March 2012");
            var results3 = grm.GetAvailableProducts("ITunes", "1st March 2012");

            // Assert - All should return same results
            Assert.Equal(4, results1.Count);
            Assert.Equal(4, results2.Count);
            Assert.Equal(4, results3.Count);
        }
    }
}

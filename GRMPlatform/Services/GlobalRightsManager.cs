using System;
using System.Collections.Generic;
using System.Linq;
using GRMPlatform.Models;
using GRMPlatform.Utilities;
namespace GRMPlatform.Services
{
    /// <summary>
    /// Core business logic for the Global Rights Management platform
    /// Filters music contracts based on partner requirements and effective dates
    /// </summary>
    public class GlobalRightsManager
    {
        private readonly List<MusicContract> musicContracts;
        private readonly List<PartnerContract> partnerContracts;
        
        // Cache parsed dates to avoid re-parsing
        private readonly Dictionary<MusicContract, (DateTime startDate, DateTime? endDate)> parsedDates;

        /// <summary>
        /// Constructor - uses dependency injection for testability
        /// Pre-parses all contract dates for performance
        /// </summary>
        public GlobalRightsManager(
            List<MusicContract> musicContracts, 
            List<PartnerContract> partnerContracts)
        {
            // Validate inputs - fail fast with clear error messages
            this.musicContracts = musicContracts 
                ?? throw new ArgumentNullException(nameof(musicContracts));
            this.partnerContracts = partnerContracts 
                ?? throw new ArgumentNullException(nameof(partnerContracts));
            
            // Pre-parse all dates once during initialization
            parsedDates = new Dictionary<MusicContract, (DateTime, DateTime?)>();
            
            foreach (var contract in this.musicContracts)
            {
                DateTime startDate = DateParser.Parse(contract.StartDate);
                DateTime? endDate = null;
                
                if (!string.IsNullOrWhiteSpace(contract.EndDate))
                {
                    endDate = DateParser.Parse(contract.EndDate);
                }

                parsedDates[contract] = (startDate, endDate);
            }
        }

        /// <summary>
        /// Gets all products available for a specific partner on a given date
        /// </summary>
        public List<ProductAvailability> GetAvailableProducts(string partnerName, string effectiveDate)
        {
            var results = new List<ProductAvailability>();

            //Find the partner and get their supported usage type
            var partner = partnerContracts.FirstOrDefault(p => 
                p.Partner.Equals(partnerName, StringComparison.OrdinalIgnoreCase));

            if (partner == null)
            {
                throw new ArgumentException($"Partner '{partnerName}' not found");
            }

            //Parse the query date (only once)
            DateTime queryDate = DateParser.Parse(effectiveDate);

            //Filter music contracts
            foreach (var contract in musicContracts)
            {
                // Check if this contract supports the partner's usage type
                if (!contract.Usages.Any(u => u.Equals(partner.Usage, StringComparison.OrdinalIgnoreCase)))
                {
                    continue; // Skip this contract - wrong usage type
                }

                // Get pre-parsed dates
                var (contractStartDate, contractEndDate) = parsedDates[contract];

                // Check if the query date falls within the contract period
                bool isAfterStart = queryDate >= contractStartDate;
                bool isBeforeEnd = !contractEndDate.HasValue || queryDate <= contractEndDate.Value;

                if (isAfterStart && isBeforeEnd)
                {
                    // This contract is valid ,Add it to results
                    results.Add(new ProductAvailability
                    {
                        Artist = contract.Artist,
                        Title = contract.Title,
                        Usage = partner.Usage,
                        StartDate = contract.StartDate,
                        EndDate = contract.EndDate
                    });
                }
            }

            //Sort results by Artist (A-Z), then Title (A-Z)
            return results.OrderBy(r => r.Artist).ThenBy(r => r.Title).ToList();
        }
    }
}
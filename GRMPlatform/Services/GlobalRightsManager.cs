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
        private readonly List<MusicContract> _musicContracts;
        private readonly List<PartnerContract> _partnerContracts;

        /// <summary>
        /// Constructor - uses dependency injection for testability
        /// </summary>
        /// <param name="musicContracts">List of all music contracts</param>
        /// <param name="partnerContracts">List of all partner contracts</param>
        public GlobalRightsManager(
            List<MusicContract> musicContracts, 
            List<PartnerContract> partnerContracts)
        {
            _musicContracts = musicContracts;
            _partnerContracts = partnerContracts;
        }

        /// <summary>
        /// Gets all products available for a specific partner on a given date
        /// </summary>
        /// <param name="partnerName">Partner name (e.g., "ITunes", "YouTube")</param>
        /// <param name="effectiveDate">Date to check (e.g., "1st March 2012")</param>
        /// <returns>List of available products, sorted by Artist then Title</returns>
        public List<ProductAvailability> GetAvailableProducts(string partnerName, string effectiveDate)
        {
            var results = new List<ProductAvailability>();

            // STEP 1: Find the partner and get their supported usage type
            var partner = _partnerContracts.FirstOrDefault(p => 
                p.Partner.Equals(partnerName, StringComparison.OrdinalIgnoreCase));

            if (partner == null)
            {
                throw new ArgumentException($"Partner '{partnerName}' not found");
            }

            // STEP 2: Parse the query date
            DateTime queryDate = DateParser.Parse(effectiveDate);

            // STEP 3: Filter music contracts
            foreach (var contract in _musicContracts)
            {
                // Check if this contract supports the partner's usage type
                // Example: ITunes needs "digital download", so check if contract has that usage
                if (!contract.Usages.Any(u => u.Equals(partner.Usage, StringComparison.OrdinalIgnoreCase)))
                {
                    continue; // Skip this contract - wrong usage type
                }

                // Parse the contract's start date
                DateTime contractStartDate = DateParser.Parse(contract.StartDate);
                DateTime? contractEndDate = null;

                // Parse end date if it exists
                if (!string.IsNullOrWhiteSpace(contract.EndDate))
                {
                    contractEndDate = DateParser.Parse(contract.EndDate);
                }

                // Check if the query date falls within the contract period
                // Must be: StartDate <= QueryDate <= EndDate (or no end date)
                bool isAfterStart = queryDate >= contractStartDate;
                bool isBeforeEnd = !contractEndDate.HasValue || queryDate <= contractEndDate.Value;

                if (isAfterStart && isBeforeEnd)
                {
                    // This contract is valid! Add it to results
                    results.Add(new ProductAvailability
                    {
                        Artist = contract.Artist,
                        Title = contract.Title,
                        Usage = partner.Usage,  // Use the partner's usage, not all of them
                        StartDate = contract.StartDate,
                        EndDate = contract.EndDate
                    });
                }
            }

            // STEP 4: Sort results by Artist (A-Z), then Title (A-Z)
            return results.OrderBy(r => r.Artist).ThenBy(r => r.Title).ToList();
        }
    }
}
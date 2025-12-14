using System.Collections.Generic;

namespace GRMPlatform.Models
{
    /// <summary>
    /// Represents a music contract with an artist
    /// Contains information about usage rights and date validity
    /// </summary>
    public class MusicContract
    {
        /// <summary>
        /// Artist name (e.g., "Tinie Tempah")
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Song title (e.g., "Frisky (Live from SoHo)")
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// List of allowed usages (e.g., ["digital download", "streaming"])
        /// </summary>
        public List<string> Usages { get; set; }

        /// <summary>
        /// Contract start date in format "1st Feb 2012"
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Contract end date (optional) in format "31st Dec 2012"
        /// Null means contract has no end date
        /// </summary>
        public string EndDate { get; set; }
    }
}
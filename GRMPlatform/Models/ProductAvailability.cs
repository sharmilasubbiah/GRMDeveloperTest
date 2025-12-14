namespace GRMPlatform.Models
{
    /// <summary>
    /// Represents a product that is available for a specific partner
    /// This is the output model that gets displayed to the user
    /// </summary>
    public class ProductAvailability
    {
        /// <summary>
        /// Artist name
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Song title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Usage type (the partner's specific usage)
        /// </summary>
        public string Usage { get; set; }

        /// <summary>
        /// Contract start date
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Contract end date (can be empty string if no end date)
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Formats the output as pipe-delimited string
        /// Format: Artist|Title|Usage|StartDate|EndDate
        /// </summary>
        public override string ToString()
        {
            return $"{Artist}|{Title}|{Usage}|{StartDate}|{EndDate ?? ""}";
        }
    }
}
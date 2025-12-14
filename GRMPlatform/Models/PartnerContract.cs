namespace GRMPlatform.Models
{
    /// <summary>
    /// Represents a distribution partner and their supported usage type
    /// </summary>
    public class PartnerContract
    {
        /// <summary>
        /// Partner name (e.g., "ITunes", "YouTube")
        /// </summary>
        public string Partner { get; set; }

        /// <summary>
        /// The type of usage this partner supports
        /// (e.g., "digital download" or "streaming")
        /// </summary>
        public string Usage { get; set; }
    }
}
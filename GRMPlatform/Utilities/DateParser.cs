using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GRMPlatform.Utilities
{
    /// <summary>
    /// Utility class for parsing custom date formats
    /// Handles dates like "1st Feb 2012", "25st Dec 2012", etc.
    /// </summary>
    public static class DateParser
    {
        /// <summary>
        /// Parses a date string with ordinal suffixes (1st, 2nd, 3rd, etc.)
        /// </summary>
        /// <param name="dateString">Date string like "1st Feb 2012"</param>
        /// <returns>DateTime object</returns>
        /// <exception cref="FormatException">Thrown when date format is invalid</exception>
        public static DateTime Parse(string dateString)
        {
            // Remove ordinal suffixes (st, nd, rd, th) from the date
            // "1st Feb 2012" becomes "1 Feb 2012"
            // "25st Dec 2012" becomes "25 Dec 2012" (handles typos too)
            string normalized = Regex.Replace(dateString, @"(\d+)(st|nd|rd|th)", "$1");

            // Try to parse using various common date formats
            string[] formats = {
                "d MMM yyyy",      // 1 Feb 2012
                "dd MMM yyyy",     // 25 Feb 2012
                "d MMMM yyyy",     // 1 February 2012
                "dd MMMM yyyy"     // 25 February 2012
            };

            DateTime result;
            if (DateTime.TryParseExact(normalized, formats, CultureInfo.InvariantCulture, 
                DateTimeStyles.None, out result))
            {
                return result;
            }

            // If parsing failed, throw a clear error message
            throw new FormatException($"Unable to parse date: {dateString}");
        }
    }
}
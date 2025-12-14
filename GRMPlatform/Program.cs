using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GRMPlatform.Models;
using GRMPlatform.Services;

namespace GRMPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // STEP 1: Validate command line arguments
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: GRMPlatform <Partner> <Date>");
                    Console.WriteLine("Example: GRMPlatform ITunes \"1st March 2012\"");
                    return;
                }

                // Parse arguments
                string partner = args[0];
                string dateInput = string.Join(" ", args.Skip(1));

                // STEP 2: Load data from text files
                var musicContracts = LoadMusicContracts("MusicContracts.txt");
                var partnerContracts = LoadPartnerContracts("PartnerContracts.txt");

                // STEP 3: Create the rights manager and query for products
                var grm = new GlobalRightsManager(musicContracts, partnerContracts);
                var results = grm.GetAvailableProducts(partner, dateInput);

                // STEP 4: Output results in the required format
                Console.WriteLine("Artist|Title|Usage|StartDate|EndDate");
                foreach (var result in results)
                {
                    Console.WriteLine(result.ToString());
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: File not found - {ex.Message}");
                Console.WriteLine("Make sure MusicContracts.txt and PartnerContracts.txt are in the same folder as the executable.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads music contracts from a pipe-delimited text file
        /// Format: Artist|Title|Usages|StartDate|EndDate
        /// </summary>
        static List<MusicContract> LoadMusicContracts(string filePath)
        {
            var contracts = new List<MusicContract>();
            
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Music contracts file not found: {filePath}");
            }

            var lines = File.ReadAllLines(filePath);

            // Skip the header line (line 0)
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split('|');
                if (parts.Length >= 4)
                {
                    // Parse the usages - they're comma-separated
                    var usages = parts[2].Split(',')
                        .Select(u => u.Trim())
                        .ToList();

                    // End date is optional (can be empty)
                    var endDate = parts.Length > 4 && !string.IsNullOrWhiteSpace(parts[4]) 
                        ? parts[4].Trim() 
                        : null;

                    contracts.Add(new MusicContract
                    {
                        Artist = parts[0].Trim(),
                        Title = parts[1].Trim(),
                        Usages = usages,
                        StartDate = parts[3].Trim(),
                        EndDate = endDate
                    });
                }
            }

            return contracts;
        }

        /// <summary>
        /// Loads partner contracts from a pipe-delimited text file
        /// Format: Partner|Usage
        /// </summary>
        static List<PartnerContract> LoadPartnerContracts(string filePath)
        {
            var contracts = new List<PartnerContract>();
            
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Partner contracts file not found: {filePath}");
            }

            var lines = File.ReadAllLines(filePath);

            // Skip the header line (line 0)
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split('|');
                if (parts.Length >= 2)
                {
                    contracts.Add(new PartnerContract
                    {
                        Partner = parts[0].Trim(),
                        Usage = parts[1].Trim()
                    });
                }
            }

            return contracts;
        }
    }
}

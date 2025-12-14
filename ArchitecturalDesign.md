Problem Analysis
Understanding the Requirements
The GRM platform needs to:

Read music contracts and partner contracts from text files
Filter music contracts based on:

Partner's supported usage type (digital download or streaming)

Effective date (must be within contract date range)
    Output matching contracts in a specific format
    Sort results by Artist, then Title

Key Challenges Identified

Custom Date Format: "1st Feb 2012", "25st Dec 2012" (non-standard)
Multiple Usages: A contract can have multiple usage types
Optional End Dates: Contracts may or may not have end dates
Case Sensitivity: Partner names should be case-insensitive
Testability: Must be unit testable
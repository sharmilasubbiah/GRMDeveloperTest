## Problem Analysis

### Understanding the Requirements

The Global Rights Management (GRM) platform needs to:

- Read **music contracts** and **partner contracts** from text files
- Filter music contracts based on:
  - The partner’s supported **usage type** (digital download or streaming)
  - The **effective date**, which must fall within the contract’s active date range
- Output matching contracts in a specified pipe-delimited format
- Sort results by **Artist**, then **Title**

---

### Key Challenges Identified

- **Custom Date Format**  
  Dates are provided in non-standard formats such as `1st Feb 2012` and `25st Dec 2012`, requiring custom parsing.

- **Multiple Usages per Contract**  
  A single music contract may support multiple usage types and must be expanded into individual output rows.

- **Optional End Dates**  
  Contracts may have no end date, indicating an open-ended agreement.

- **Case Insensitivity**  
  Partner names should be matched in a case-insensitive manner.

- **Testability**  
  The solution must be designed to support unit testing and clear separation of concerns.


Understanding the Data:

### MusicContracts.txt Structure:
```
Artist | Title | Usages | StartDate | EndDate
───────────────────────────────────────────────────
Tinie Tempah | Frisky | digital download, streaming | 1st Feb 2012 | (no end)
Monkey Claw | Christmas Special | streaming | 25st Dec 2012 | 31st Dec 2012
```

**Key Points:**
- Multiple usages separated by commas: `digital download, streaming`
- Empty EndDate means contract has no end date
- Dates use custom format: `1st Feb 2012`
- ITunes only distributes digital downloads
- YouTube only does streaming

### PartnerContracts.txt Structure:
```
Partner | Usage
─────────────────────
ITunes  | digital download
YouTube | streaming





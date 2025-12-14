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

## 📁 Project Structure

```text
GRMDeveloperTest/
├── GRMPlatform/                  # Main Console Application
│   ├── Models/                   # Data Models
│   │   ├── MusicContract.cs
│   │   ├── PartnerContract.cs
│   │   └── ProductAvailability.cs
│   ├── Services/                 # Business Logic
│   │   └── GlobalRightsManager.cs
│   ├── Utilities/                # Helper Classes
│   │   └── DateParser.cs
│   ├── Program.cs                # Entry Point
│   ├── MusicContracts.txt        # Test Data
│   ├── PartnerContracts.txt      # Test Data
│   └── GRMPlatform.csproj
│
├── GRMPlatform.Tests/             # Unit Tests
│   ├── GlobalRightsManagerTests.cs
│   ├── DateParserTests.cs
│   └── GRMPlatform.Tests.csproj
│
├── GRMPlatform.sln                # Solution File
├── ArchitecturalDesign.md         # Architecture & Design Overview
├── ACCEPTANCE_CRITIRIA.md         # Acceptance Critiria
├── README.md                      # Project ReadMe
├── QUICK_TEST.md                  # Quick Test Guide
└── verify-repo.sh                 # Repository Verification Script

```

**Layered Architecture:**
- **Presentation Layer**: `Program.cs` - Handles I/O and CLI
- **Business Logic Layer**: `GlobalRightsManager` - Core filtering logic
- **Utility Layer**: `DateParser` - Custom date parsing
- **Data Layer**: Models - Simple POCOs

**Design Principles Applied:**
- **Single Responsibility Principle** - Each class has one clear purpose
- **Dependency Injection** - Testable design via constructor injection
- **Separation of Concerns** - Clear boundaries between layers
- **Open/Closed Principle** - Easy to extend without modification



### Key Components

#### 1. GlobalRightsManager
**Purpose**: Core business logic for contract filtering

**Key Features:**
- Two-stage filtering (usage type, then date range)
- Case-insensitive partner matching
- Handles optional end dates
- Returns sorted results

#### 2. DateParser
**Purpose**: Parses custom date format with ordinal suffixes

**Handles:**
- "1st Feb 2012" → DateTime(2012, 2, 1)
- "25st Dec 2012" → DateTime(2012, 12, 25) (handles typos)
- Supports both short ("Feb") and long ("February") month names

#### 3. Models
**Purpose**: Clean data structures with no business logic
- `MusicContract` - Artist agreements
- `PartnerContract` - Distribution partner info
- `ProductAvailability` - Output format

---


## Design Decisions & Trade-offs

### 1. Custom Date Parser
**Decision**: Created dedicated `DateParser` utility class

**Rationale**: Standard `DateTime.Parse()` doesn't handle ordinal suffixes ("1st", "2nd", "3rd")

**Implementation**: Regex to strip suffixes, then standard parsing

### 2. In-Memory Processing
**Decision**: Load all data into memory

**Pros**: Simple, fast for small datasets  
**Cons**: Limited scalability for large datasets  
**Trade-off**: Appropriate for this use case; can be replaced with database later

### 3. Dependency Injection
**Decision**: Pass contracts via constructor

**Pros**: Testable, no hidden dependencies  
**Cons**: Slightly more verbose  
**Trade-off**: Worth it for testability and maintainability

### 4. Two-Stage Filtering
**Decision**: Filter by usage first, then by date

**Rationale**: Skip expensive date parsing if usage doesn't match

**Performance**: O(n) complexity - linear in number of contracts

---

## Error Handling

The application handles:
- **Missing files**: Clear error message with file location
- **Invalid partner**: ArgumentException with partner name
- **Invalid date format**: FormatException with input value
- **Malformed data**: Skips invalid lines, processes valid ones

**Example Error Messages:**
```bash
Error: Partner 'Spotify' not found
Error: Unable to parse date: Invalid Date
Error: File not found - MusicContracts.txt
```

---





# Acceptance Criteria - GRM Platform

## Overview

This document defines the acceptance criteria for the Global Rights Management (GRM) Platform solution, based on the original specification requirements.

---

## Functional Requirements

### FR1: File Input Processing
**Given** the application has access to two pipe-delimited text files  
**When** the application starts  
**Then** it must successfully read and parse:
- `MusicContracts.txt` (Artist|Title|Usages|StartDate|EndDate)
- `PartnerContracts.txt` (Partner|Usage)

**Acceptance Tests:**
-  Application reads MusicContracts.txt without errors
-  Application reads PartnerContracts.txt without errors
-  Application handles missing files with clear error messages
-  Application parses pipe-delimited format correctly
-  Application handles comma-separated usages (e.g., "digital download, streaming")

---

### FR2: Command Line Interface
**Given** the user wants to query available products  
**When** the user runs the application with partner name and date  
**Then** the application must accept command line arguments in the format:
```bash
GRMPlatform <Partner> <Date>
```

**Acceptance Tests:**
-  Application accepts partner name as first argument
-  Application accepts date as second argument (with spaces)
-  Application shows usage instructions when arguments are missing
-  Application handles multi-word dates (e.g., "1st March 2012")

---

### FR3: Partner Usage Filtering
**Given** a partner name is provided  
**When** the application queries for available products  
**Then** it must filter music contracts to match the partner's supported usage type

**Acceptance Tests:**
-  ITunes returns only "digital download" products
-  YouTube returns only "streaming" products
-  Application is case-insensitive for partner names (ITunes, ITUNES, itunes)
-  Application throws clear error for unknown partners

---

### FR4: Date Range Validation
**Given** an effective date is provided  
**When** the application queries for available products  
**Then** it must only return contracts where:
- Start date ≤ Effective date
- End date ≥ Effective date (or no end date exists)

**Acceptance Tests:**
-  Contracts starting after the query date are excluded
-  Contracts ending before the query date are excluded
-  Contracts with no end date are included if start date is valid
-  Time-limited contracts (with end dates) are handled correctly

---

### FR5: Output Formatting
**Given** available products are found  
**When** the application displays results  
**Then** the output must be:
- Pipe-delimited format: `Artist|Title|Usage|StartDate|EndDate`
- Header row: `Artist|Title|Usage|StartDate|EndDate`
- Sorted alphabetically by Artist, then by Title
- Empty string for missing end dates

**Acceptance Tests:**
-  Output has correct header row
-  Each result is pipe-delimited
-  Results are sorted by Artist (primary), Title (secondary)
-  End date column is empty when no end date exists

---

### FR6: Custom Date Format Support
**Given** dates are provided in format "Xst/nd/rd/th Month Year"  
**When** the application parses dates  
**Then** it must correctly handle:
- Ordinal suffixes (1st, 2nd, 3rd, 4th, etc.)
- Short month names (Feb, Mar, Dec)
- Long month names (February, March, December)
- Typos in ordinal suffixes (e.g., "25st" instead of "25th")

**Acceptance Tests:**
-  Parses "1st Feb 2012" correctly
-  Parses "25st Dec 2012" correctly (handles typo)
-  Parses both short and long month names
-  Throws clear error for invalid date formats

---

## Required Test Scenarios

### Scenario 1: ITunes on 1st March 2012
**Given** partner is "ITunes" and date is "1st March 2012"  
**When** the application runs  
**Then** it must return exactly 4 digital download products:
```
Artist|Title|Usage|StartDate|EndDate
Monkey Claw|Black Mountain|digital download|1st Feb 2012|
Monkey Claw|Motor Mouth|digital download|1st Mar 2011|
Tinie Tempah|Frisky (Live from SoHo)|digital download|1st Feb 2012|
Tinie Tempah|Miami 2 Ibiza|digital download|1st Feb 2012|
```

**Acceptance Test:** Output matches exactly (including sort order)

---

### Scenario 2: YouTube on 1st April 2012
**Given** partner is "YouTube" and date is "1st April 2012"  
**When** the application runs  
**Then** it must return exactly 2 streaming products:
```
Artist|Title|Usage|StartDate|EndDate
Monkey Claw|Motor Mouth|streaming|1st Mar 2011|
Tinie Tempah|Frisky (Live from SoHo)|streaming|1st Feb 2012|
```

**Acceptance Test:** Output matches exactly (including sort order)

---

### Scenario 3: YouTube on 27th Dec 2012
**Given** partner is "YouTube" and date is "27th Dec 2012"  
**When** the application runs  
**Then** it must return exactly 4 streaming products (including time-limited):
```
Artist|Title|Usage|StartDate|EndDate
Monkey Claw|Christmas Special|streaming|25st Dec 2012|31st Dec 2012
Monkey Claw|Iron Horse|streaming|1st June 2012|
Monkey Claw|Motor Mouth|streaming|1st Mar 2011|
Tinie Tempah|Frisky (Live from SoHo)|streaming|1st Feb 2012|
```

**Acceptance Test:** Output matches exactly (includes Christmas Special within date range)

---

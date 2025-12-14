# GRM Platform - Global Rights Management Solution

##  Requirements Met

### Submission Checklist

- [x] **Visual Studio Solution** - `GRMPlatform.sln`
- [x] **Executable Console Application** - Command-line interface
- [x] **Source Code** - 7 well-structured C# classes
- [x] **Unit Tests** - 16 comprehensive test cases using xUnit
- [x] **All Test Scenarios Pass** - Scenarios 1, 2, and 3 verified

### Functional Requirements

- [x] Reads music contracts and partner contracts from text files
- [x] Accepts partner name and effective date as command-line arguments
- [x] Filters contracts by partner usage type (digital download vs streaming)
- [x] Validates contracts against date ranges (start/end dates)
- [x] Outputs results in pipe-delimited format
- [x] Sorts results alphabetically by Artist, then Title
- [x] Handles optional end dates (contracts with no expiry)

---

## Quick Start

### Prerequisitess
- .NET 6.0 SDK or later
- macOS, Linux, or Windows

### Installation
```bash
# Clone the repository
git clone https://github.com/sharmilasubbiah/GRMDeveloperTest.git
cd GRMDeveloperTest

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test
```

### Running the Application
```bash
# General syntax
dotnet run --project GRMPlatform -- <Partner> "<Date>"

# Test Scenario 1: ITunes on 1st March 2012
dotnet run --project GRMPlatform -- ITunes "1st March 2012"

# Test Scenario 2: YouTube on 1st April 2012
dotnet run --project GRMPlatform -- YouTube "1st April 2012"

# Test Scenario 3: YouTube on 27th Dec 2012
dotnet run --project GRMPlatform -- YouTube "27th Dec 2012"
```

---

## Test Results

### Test Scenario 1: ITunes 1st March 2012
**Expected Output:**
```
Artist|Title|Usage|StartDate|EndDate
Monkey Claw|Black Mountain|digital download|1st Feb 2012|
Monkey Claw|Motor Mouth|digital download|1st Mar 2011|
Tinie Tempah|Frisky (Live from SoHo)|digital download|1st Feb 2012|
Tinie Tempah|Miami 2 Ibiza|digital download|1st Feb 2012|
```
**Result**: PASS - Returns 4 digital download products

### Test Scenario 2: YouTube 1st April 2012
**Expected Output:**
```
Artist|Title|Usage|StartDate|EndDate
Monkey Claw|Motor Mouth|streaming|1st Mar 2011|
Tinie Tempah|Frisky (Live from SoHo)|streaming|1st Feb 2012|
```
**Result**: PASS - Returns 2 streaming products

### Test Scenario 3: YouTube 27th Dec 2012
**Expected Output:**
```
Artist|Title|Usage|StartDate|EndDate
Monkey Claw|Christmas Special|streaming|25st Dec 2012|31st Dec 2012
Monkey Claw|Iron Horse|streaming|1st June 2012|
Monkey Claw|Motor Mouth|streaming|1st Mar 2011|
Tinie Tempah|Frisky (Live from SoHo)|streaming|1st Feb 2012|
```
**Result**: PASS - Returns 4 streaming products (including time-limited contract)

### Unit Test Summary
```
Total Tests: 24
Passed: 24
Failed: 0
Success Rate: 100%
```

---


## 🧪 Testing Strategy

### Test Coverage

**Functional Tests (Required Scenarios):**
1. ITunes 1st March 2012
2. YouTube 1st April 2012
3. YouTube 27th Dec 2012

**Edge Case Tests:**
4. Date before contract starts (should return empty)
5. Date after contract ends (excludes expired)
6. Invalid partner name (throws ArgumentException)
7. Case-insensitive partner matching

**DateParser Tests:**
8-16. 9 different date format variations

### Running Tests
```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test
dotnet test --filter "TestScenario1_ITunes_1stMarch2012"
```

---


## 🛠️ Development Setup

### Using Visual Studio for Mac
```bash
# Open the solution
open GRMPlatform.sln
```

### Using VS Code
```bash
# Open in VS Code
code .

# Install C# extension
# Press Cmd+Shift+P → "Extensions: Install Extensions" → Search "C#"
```

### Using Command Line Only
```bash
# Build
dotnet build

# Run
dotnet run --project GRMPlatform -- ITunes "1st March 2012"

# Test
dotnet test
```

---


## Documentation

- **ArchitecturalDesign.md** - Comprehensive architecture diagrams and design decisions
- **README.md** - This file
- **ACCEPTANCE_CRITIRIA** - Ascceptance criteria for the Global Rights Management (GRM) Platform solution, based on the original specification requirements.
- **Code Comments** - Inline XML documentation
- **Git History** - Clear commit messages showing development progress

---

## Learning Outcomes

This implementation demonstrates:
- Clean architecture principles
- Test-driven development
- SOLID design principles
- File I/O operations
- Custom parsing logic
- Error handling strategies
- Unit testing with xUnit
- Command-line application design
- Git workflow

---
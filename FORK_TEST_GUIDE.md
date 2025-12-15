# Fork & Clone Test Guide

This guide helps verify the GRM Platform works correctly after forking/cloning.

---

## Quick Test (5 Minutes)

### Step 1: Clone the Repository
```bash
git clone https://github.com/sharmilasubbiah/GRMDeveloperTest.git
cd GRMDeveloperTest
```
---

### Step 2: Verify Prerequisites
```bash
# Check .NET is installed
dotnet --version

# Should show 6.0 or later (e.g., 6.0.x, 8.0.x, 9.0.x)
```

**If .NET is not installed:**
- **Mac**: `brew install --cask dotnet-sdk`
- **Windows**: Download from https://dotnet.microsoft.com/download
- **Linux**: Follow https://docs.microsoft.com/dotnet/core/install/linux

---

### Step 3: Restore & Build
```bash
# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build
```

**Expected Output:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:03.xx
```

 **Success**: Build completed without errors

---

### Step 4: Run Tests
```bash
dotnet test
```

**Expected Output:**
```
Test summary: total: 24, failed: 0, succeeded: 24, skipped: 0
```

 **Success**: All 24 tests passing

---

### Step 5: Run All 3 Test Scenarios

#### Scenario 1: ITunes 1st March 2012
```bash
dotnet run --project GRMPlatform -- ITunes "1st March 2012"
```

**Expected Output:**
```
Artist|Title|Usage|StartDate|EndDate
Monkey Claw|Black Mountain|digital download|1st Feb 2012|
Monkey Claw|Motor Mouth|digital download|1st Mar 2011|
Tinie Tempah|Frisky (Live from SoHo)|digital download|1st Feb 2012|
Tinie Tempah|Miami 2 Ibiza|digital download|1st Feb 2012|
```

 **Success**: 4 results, correctly sorted

---

#### Scenario 2: YouTube 1st April 2012
```bash
dotnet run --project GRMPlatform -- YouTube "1st April 2012"
```

**Expected Output:**
```
Artist|Title|Usage|StartDate|EndDate
Monkey Claw|Motor Mouth|streaming|1st Mar 2011|
Tinie Tempah|Frisky (Live from SoHo)|streaming|1st Feb 2012|
```

**Success**: 2 results, correctly sorted

---

#### Scenario 3: YouTube 27th Dec 2012
```bash
dotnet run --project GRMPlatform -- YouTube "27th Dec 2012"
```

**Expected Output:**
```
Artist|Title|Usage|StartDate|EndDate
Monkey Claw|Christmas Special|streaming|25st Dec 2012|31st Dec 2012
Monkey Claw|Iron Horse|streaming|1st June 2012|
Monkey Claw|Motor Mouth|streaming|1st Mar 2011|
Tinie Tempah|Frisky (Live from SoHo)|streaming|1st Feb 2012|
```

**Success**: 4 results including time-limited "Christmas Special"

---

## All Tests Passed?

If all 5 steps completed successfully:

** SUCCESS! The fork works perfectly!**

You have successfully verified:
- Build system works
- All dependencies restored
- All unit tests pass
- All 3 scenarios produce correct output
- Application is ready to use

---

##  Troubleshooting

### Issue: "File not found: MusicContracts.txt"

**Cause**: Application can't locate data files

**Solution:**
```bash
# Verify files exist in repository
ls -la GRMPlatform/*.txt

# Should see:
# MusicContracts.txt
# PartnerContracts.txt

# If missing, the repository may be incomplete
git status
```

---

### Issue: "dotnet: command not found"

**Cause**: .NET SDK not installed

**Solution:** Install .NET SDK for your platform:
- **Mac**: `brew install --cask dotnet-sdk`
- **Windows**: https://dotnet.microsoft.com/download
- **Linux**: https://docs.microsoft.com/dotnet/core/install/linux

After installation, verify:
```bash
dotnet --version
```

---

### Issue: Build errors about nullable warnings

**Cause**: Nullable reference types enabled in newer .NET versions

**Solution:** This shouldn't happen as nullable is disabled in .csproj, but if it does:
```bash
# Clean and rebuild
dotnet clean
rm -rf */bin */obj
dotnet restore
dotnet build
```

---

### Issue: Tests fail with "Partner not found"

**Cause**: Data files not loaded correctly

**Solution:**
```bash
# Check data files exist
cat GRMPlatform/MusicContracts.txt
cat GRMPlatform/PartnerContracts.txt

# Rebuild to ensure files are copied to output
dotnet clean
dotnet build
```

---

### Issue: Wrong .NET version (net9.0 instead of net6.0)

**Cause**: System has newer .NET version installed

**Solution:** This is actually fine! The project targets net6.0 but will build with newer versions due to forward compatibility. The application will work correctly regardless of the build target shown.

---

## What Should Work After Fork

After successfully forking/cloning, you should have:

All source files (.cs files)  
Test data files (MusicContracts.txt, PartnerContracts.txt)  
Project files (.csproj, .sln)  
All tests passing (24/24)  
All 3 scenarios producing correct output  
No build errors or warnings  
Complete documentation

---

## Quick Verification One-Liner

Run all tests at once:
```bash
dotnet restore && \
dotnet build && \
dotnet test && \
echo "Scenario 1:" && dotnet run --project GRMPlatform -- ITunes "1st March 2012" && \
echo -e "\nScenario 2:" && dotnet run --project GRMPlatform -- YouTube "1st April 2012" && \
echo -e "\nScenario 3:" && dotnet run --project GRMPlatform -- YouTube "27th Dec 2012"
```

This will:
1. Restore packages
2. Build solution
3. Run all tests
4. Execute all 3 scenarios

If all complete successfully, the fork is perfect! 

---

## Alternative: Use Verification Script

If the repository includes `verify-repo.sh`:
```bash
# Make executable (if not already)
chmod +x verify-repo.sh

# Run verification
./verify-repo.sh
```

This automated script checks:
- File existence
- Build success
- Test pass rate
- All scenarios

---

##  Next Steps

After successful verification:

1. **Read the documentation**:
   - `README.md` - Setup and usage guide
   - `ArchitecturalDesign.md` - Design decisions
   - `ACCEPTANCE_CRITERIA.md` - Requirements verification

2. **Explore the code**:
   - Start with `Program.cs` (entry point)
   - Look at `GlobalRightsManager.cs` (core logic)
   - Check out the test files for examples

3. **Try custom queries**:
```bash
   # Case-insensitive partner names
   dotnet run --project GRMPlatform -- ITUNES "1st March 2012"
   
   # Different dates
   dotnet run --project GRMPlatform -- YouTube "1st Jan 2013"
   
   # Invalid partner (see error handling)
   dotnet run --project GRMPlatform -- Spotify "1st March 2012"
```

---

## Verification Checklist

Use this checklist when testing a fork:

- [ ] Repository cloned successfully
- [ ] .NET SDK installed (6.0+)
- [ ] `dotnet restore` completed without errors
- [ ] `dotnet build` completed without errors
- [ ] `dotnet test` shows 24/24 tests passing
- [ ] Scenario 1 produces 4 correct results
- [ ] Scenario 2 produces 2 correct results
- [ ] Scenario 3 produces 4 correct results
- [ ] Data files (MusicContracts.txt, PartnerContracts.txt) present
- [ ] Documentation files readable

---

**Status**: Fork verification complete  
**Ready for**: Development, Review, Interview Discussion

---


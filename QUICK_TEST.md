# ✅ Quick Test Checklist

Run these commands in order:

## Build
```bash
dotnet build
```
**Expected**: Build succeeded

## Test
```bash
dotnet test
```
**Expected**: All tests passing (24 tests)

## Scenario 1
```bash
dotnet run --project GRMPlatform -- ITunes "1st March 2012"
```
**Expected**: 4 results including "Monkey Claw|Black Mountain"

## Scenario 2
```bash
dotnet run --project GRMPlatform -- YouTube "1st April 2012"
```
**Expected**: 2 results including "Monkey Claw|Motor Mouth"

## Scenario 3
```bash
dotnet run --project GRMPlatform -- YouTube "27th Dec 2012"
```
**Expected**: 4 results including "Christmas Special"

---

## All Pass?

If all 5 commands work, the fork is perfect!

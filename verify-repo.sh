#!/bin/bash

echo "🔍 GRM Platform Repository Verification"
echo "========================================"
echo ""

GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m'

PASS="${GREEN}✅ PASS${NC}"
FAIL="${RED}❌ FAIL${NC}"

TOTAL=0
PASSED=0

check() {
    TOTAL=$((TOTAL + 1))
    if [ $? -eq 0 ]; then
        echo -e "$PASS - $1"
        PASSED=$((PASSED + 1))
    else
        echo -e "$FAIL - $1"
    fi
}

echo "�� Checking essential files..."
[ -f "GRMPlatform.sln" ] && check "Solution file exists" || check "Solution file exists"
[ -f "GRMPlatform/MusicContracts.txt" ] && check "MusicContracts.txt exists" || check "MusicContracts.txt exists"
[ -f "GRMPlatform/PartnerContracts.txt" ] && check "PartnerContracts.txt exists" || check "PartnerContracts.txt exists"

echo ""
echo "🔨 Building solution..."
dotnet build > /dev/null 2>&1
check "Build"

echo ""
echo "🧪 Running tests..."
dotnet test > /dev/null 2>&1
check "Tests"

echo ""
echo "🎯 Testing Scenario 1..."
dotnet run --project GRMPlatform -- ITunes "1st March 2012" > /tmp/test1.txt 2>&1
grep -q "Monkey Claw|Black Mountain" /tmp/test1.txt
check "Scenario 1"

echo ""
echo "🎯 Testing Scenario 2..."
dotnet run --project GRMPlatform -- YouTube "1st April 2012" > /tmp/test2.txt 2>&1
grep -q "Monkey Claw|Motor Mouth" /tmp/test2.txt
check "Scenario 2"

echo ""
echo "🎯 Testing Scenario 3..."
dotnet run --project GRMPlatform -- YouTube "27th Dec 2012" > /tmp/test3.txt 2>&1
grep -q "Christmas Special" /tmp/test3.txt
check "Scenario 3"

echo ""
echo "========================================"
echo "Results: $PASSED/$TOTAL checks passed"
echo "========================================"

if [ $PASSED -eq $TOTAL ]; then
    echo -e "${GREEN}🎉 All checks passed!${NC}"
else
    echo -e "${RED}⚠️  $((TOTAL - PASSED)) check(s) failed${NC}"
    echo ""
    echo "Run these commands manually to debug:"
    echo "  dotnet build"
    echo "  dotnet test --verbosity normal"
    echo "  dotnet run --project GRMPlatform -- ITunes \"1st March 2012\""
fi

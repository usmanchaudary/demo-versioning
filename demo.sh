#!/usr/bin/env bash
# =============================================================================
# GitVersion Semantic Versioning Demo Script
# =============================================================================
# This script demonstrates how GitVersion calculates versions from git history
# and stamps the .NET assembly with the computed version.
#
# Prerequisites:
#   - git
#   - .NET 8 SDK
#   - GitVersion CLI: dotnet tool install --global GitVersion.Tool
# =============================================================================

set -e

GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

show_version() {
    echo -e "${CYAN}--- GitVersion Output ---${NC}"
    dotnet-gitversion /showvariable FullSemVer 2>/dev/null || echo "(GitVersion CLI not installed — install with: dotnet tool install --global GitVersion.Tool)"
    echo ""
}

build_and_run() {
    echo -e "${CYAN}--- Building & Running .NET App ---${NC}"
    dotnet run --project DemoVersioning/DemoVersioning.csproj 2>/dev/null || echo "(dotnet build skipped — install .NET 8 SDK to see assembly version output)"
    echo ""
}

step() {
    echo -e "\n${GREEN}════════════════════════════════════════════════════════${NC}"
    echo -e "${GREEN}  STEP $1: $2${NC}"
    echo -e "${GREEN}════════════════════════════════════════════════════════${NC}"
    echo -e "${YELLOW}  $3${NC}\n"
}

# ── Step 1: Initial commit + tag ──────────────────────────────────────────────
step 1 "Initial release" "Create first commit and tag v1.0.0"

git add -A
git commit -m "chore: initial project setup"
git tag v1.0.0

show_version
build_and_run

# ── Step 2: Patch bump on main ────────────────────────────────────────────────
step 2 "Patch bump" "A bug fix commit on main increments PATCH → 1.0.1"

echo "// bug fix applied" > bugfix.txt
git add bugfix.txt
git commit -m "fix: resolve null reference in user service"

show_version

# ── Step 3: Feature branch ───────────────────────────────────────────────────
step 3 "Feature branch" "Creating feature/user-auth → pre-release tag"

git checkout -b feature/user-auth
echo "// auth module" > auth.txt
git add auth.txt
git commit -m "feat: add user authentication module"

show_version

# ── Step 4: More work on feature ──────────────────────────────────────────────
step 4 "More feature work" "Additional commit bumps pre-release counter"

echo "// auth tests" > auth-tests.txt
git add auth-tests.txt
git commit -m "test: add auth module unit tests"

show_version

# ── Step 5: Merge feature to main ────────────────────────────────────────────
step 5 "Merge feature" "Merge feature/user-auth back to main"

git checkout main
git merge feature/user-auth --no-ff -m "Merge feature/user-auth into main"

show_version
build_and_run

# ── Step 6: Major version bump ───────────────────────────────────────────────
step 6 "Major bump" "Using +semver: major in commit message → 2.0.0"

echo "// breaking API change" > breaking.txt
git add breaking.txt
git commit -m "refactor: rewrite auth API

+semver: major"

show_version
build_and_run

# ── Step 7: Release branch ───────────────────────────────────────────────────
step 7 "Release branch" "Creating release/2.0.0 → beta pre-release"

git checkout -b release/2.0.0
echo "// release prep" > release-notes.txt
git add release-notes.txt
git commit -m "chore: prepare release 2.0.0"

show_version

# ── Step 8: Hotfix branch ────────────────────────────────────────────────────
step 8 "Hotfix" "Creating hotfix/critical-fix from main → beta pre-release"

git checkout main
git checkout -b hotfix/critical-fix
echo "// critical fix" > critical-fix.txt
git add critical-fix.txt
git commit -m "fix: patch critical security vulnerability"

show_version

# ── Final: Back to main, build the app ───────────────────────────────────────
git checkout main

echo -e "\n${GREEN}════════════════════════════════════════════════════════${NC}"
echo -e "${GREEN}  DEMO COMPLETE${NC}"
echo -e "${GREEN}════════════════════════════════════════════════════════${NC}"
echo ""
echo "Branches created:"
git branch --list
echo ""
echo "Tags created:"
git tag --list
echo ""
echo "Try these commands:"
echo "  dotnet-gitversion                                    # Full JSON output"
echo "  dotnet-gitversion /showvariable FullSemVer           # Just the version"
echo "  dotnet run --project DemoVersioning/DemoVersioning.csproj  # See it in the app"
echo "  git checkout feature/user-auth && dotnet-gitversion  # Version on a branch"

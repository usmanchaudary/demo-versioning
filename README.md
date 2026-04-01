# GitVersion Semantic Versioning Demo

This project demonstrates how [GitVersion](https://gitversion.net/) automatically calculates semantic versions from your Git history.

## Prerequisites

- Git
- [GitVersion](https://gitversion.net/docs/usage/cli/installation) (install via `dotnet tool install --global GitVersion.Tool`)

## Semantic Versioning (SemVer)

```
MAJOR.MINOR.PATCH
  |     |     |
  |     |     +-- Bug fixes, no API changes
  |     +-------- New features, backwards compatible
  +-------------- Breaking changes
```

## How GitVersion Works

GitVersion reads your **Git history** (branches, tags, merge commits) and computes a version number automatically. No manual version bumping needed.

### Branch Strategies

| Branch Pattern     | Tag/Label       | Default Increment |
|--------------------|-----------------|-------------------|
| `main`             | *(stable)*      | Patch             |
| `develop`          | `alpha`         | Minor             |
| `feature/*`        | `feat-{name}`   | Inherit           |
| `release/*`        | `beta`          | None              |
| `hotfix/*`         | `beta`          | Patch             |

## Quick Start

```bash
# 1. Check current version
dotnet-gitversion

# 2. Create initial tag
git tag v1.0.0
dotnet-gitversion   # => 1.0.0

# 3. Make a commit on main (patch bump)
echo "fix" > fix.txt && git add . && git commit -m "fix: resolve null check"
dotnet-gitversion   # => 1.0.1

# 4. Create a feature branch (minor bump via develop)
git checkout -b feature/add-login
echo "login" > login.txt && git add . && git commit -m "feat: add login page"
dotnet-gitversion   # => 1.0.1-feat-add-login.1

# 5. Bump MAJOR version with commit message convention
git commit --allow-empty -m "+semver: major"
dotnet-gitversion   # => 2.0.0-feat-add-login.2
```

## Commit Message Bumps

GitVersion supports version bumps via commit messages:

| Commit message contains | Effect       |
|-------------------------|--------------|
| `+semver: major`        | Bump MAJOR   |
| `+semver: minor`        | Bump MINOR   |
| `+semver: patch`        | Bump PATCH   |
| `+semver: none`         | No bump      |

## Demo Script

Run the included `demo.sh` to see GitVersion in action across branches:

```bash
bash demo.sh
```

## Configuration

See `GitVersion.yml` for the full configuration. Key settings:

- **mode: Mainline** - version increments on every commit (no need for tags on every release)
- **tag-prefix: 'v'** - git tags like `v1.0.0` are recognized
- **assembly-versioning-scheme: MajorMinorPatch** - for .NET assembly versions

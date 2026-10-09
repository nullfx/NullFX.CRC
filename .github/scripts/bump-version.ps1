<#
.SYNOPSIS
  Computes (and if needed increments) the release version stored in Directory.Build.props.

.DESCRIPTION
  Directory.Build.props holds RELEASE_MAJOR / RELEASE_MINOR / RELEASE_REVISION for the
  most recently released or pending-release version. This script:

    1. Reads the three parts and computes RELEASE_VERSION.
    2. Checks whether that version has already been released (a git tag named
       v<version> or <version> exists; older releases were tagged without the v).
    3. If it was released, increments the requested part (patch by default),
       writes the new values back to Directory.Build.props and reports changed=true.
       If it was NOT released yet (e.g. a previous release run failed after the bump
       was committed), it keeps the version as-is and reports changed=false, so
       re-running the release never skips or double-increments a version.

  Results are written to $env:GITHUB_OUTPUT when running in GitHub Actions.

.EXAMPLE
  ./.github/scripts/bump-version.ps1 -Bump patch
#>
[CmdletBinding()]
param(
  [ValidateSet('patch', 'minor', 'major')]
  [string] $Bump = 'patch',
  [string] $PropsPath = 'Directory.Build.props'
)

$ErrorActionPreference = 'Stop'

$content = Get-Content -Path $PropsPath -Raw

function Get-Part([string] $name) {
  $m = [regex]::Match($content, "<$name\b[^>]*>\s*(\d+)\s*</$name>")
  if (-not $m.Success) { throw "Could not find <$name> in $PropsPath" }
  return [int] $m.Groups[1].Value
}

function Set-Part([string] $name, [int] $value) {
  $pattern = "(<$name\b[^>]*>)\s*\d+\s*(</$name>)"
  $script:content = [regex]::Replace($script:content, $pattern, "`${1}$value`${2}")
}

function Test-Released([string] $version) {
  try {
    foreach ($tag in @("v$version", $version)) {
      & git rev-parse -q --verify "refs/tags/$tag" *> $null
      if ($LASTEXITCODE -eq 0) { return $true }
    }
    return $false
  }
  finally {
    # 'git rev-parse --verify' exits 1 when a tag does not exist, which is the
    # expected answer here. Don't let that leak out as the step's exit code:
    # GitHub Actions' pwsh wrapper ends every step with 'exit $LASTEXITCODE'.
    $global:LASTEXITCODE = 0
  }
}

$major = Get-Part 'RELEASE_MAJOR'
$minor = Get-Part 'RELEASE_MINOR'
$revision = Get-Part 'RELEASE_REVISION'
$previous = "$major.$minor.$revision"

if (Test-Released $previous) {
  switch ($Bump) {
    'major' { $major++; $minor = 0; $revision = 0 }
    'minor' { $minor++; $revision = 0 }
    default { $revision++ }
  }
  $changed = $true
  Write-Host "Version $previous is already released; applying '$Bump' increment."
}
else {
  $changed = $false
  Write-Host "::notice::Version $previous has not been released yet; releasing it without incrementing."
}

$version = "$major.$minor.$revision"
if ($changed -and (Test-Released $version)) {
  throw "Computed version $version is already tagged. Fix RELEASE_* in $PropsPath before releasing."
}

if ($changed) {
  Set-Part 'RELEASE_MAJOR' $major
  Set-Part 'RELEASE_MINOR' $minor
  Set-Part 'RELEASE_REVISION' $revision
  # Preserve the file's existing line endings / encoding (no BOM).
  [System.IO.File]::WriteAllText((Resolve-Path $PropsPath), $content, [System.Text.UTF8Encoding]::new($false))
}

Write-Host "RELEASE_VERSION: $previous -> $version (changed=$changed)"

if ($env:GITHUB_OUTPUT) {
  @(
    "previous=$previous"
    "version=$version"
    "major=$major"
    "minor=$minor"
    "revision=$revision"
    "changed=$($changed.ToString().ToLowerInvariant())"
  ) | Add-Content -Path $env:GITHUB_OUTPUT
}

exit 0

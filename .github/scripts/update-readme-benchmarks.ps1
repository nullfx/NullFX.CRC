<#
.SYNOPSIS
  Replaces everything after the "## Benchmarks" heading in README.md with the latest
  BenchmarkDotNet GitHub-markdown report.

.DESCRIPTION
  Reads every *-report-github.md file BenchmarkDotNet wrote to the results directory
  and writes them, verbatim, below the heading. Everything above the heading is left
  untouched. The heading is appended if it is missing. The README's BOM and line
  endings are preserved.

  Writes changed=true|false to $env:GITHUB_OUTPUT when running in GitHub Actions.

.EXAMPLE
  ./.github/scripts/update-readme-benchmarks.ps1 -Version 1.1.14
#>
[CmdletBinding()]
param(
  [string] $ReadmePath = 'README.md',
  [string] $ResultsDirectory = 'artifacts/benchmarks/results',
  [string] $Heading = '## Benchmarks',
  [string] $Version
)

$ErrorActionPreference = 'Stop'

$reports = @(Get-ChildItem -Path $ResultsDirectory -Filter '*-report-github.md' -File -ErrorAction SilentlyContinue | Sort-Object Name)
if ($reports.Count -eq 0) { throw "No *-report-github.md benchmark reports found in '$ResultsDirectory'." }

$path = (Resolve-Path $ReadmePath).Path
$bytes = [System.IO.File]::ReadAllBytes($path)
$hasBom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
$original = [System.Text.UTF8Encoding]::new($false).GetString($bytes, $(if ($hasBom) { 3 } else { 0 }), $bytes.Length - $(if ($hasBom) { 3 } else { 0 }))
$newline = if ($original.Contains("`r`n")) { "`r`n" } else { "`n" }

# Everything up to and including the heading line is kept; everything after it is replaced.
$lines = $original -split '\r?\n'
$index = [Array]::FindIndex($lines, [Predicate[string]] { param($l) $l.TrimEnd() -eq $Heading })
if ($index -ge 0) {
  $head = ($lines[0..$index] -join $newline)
}
else {
  Write-Host "::notice::'$Heading' not found in $ReadmePath; appending it."
  $head = $original.TrimEnd() + $newline + $newline + $Heading
}

$body = foreach ($report in $reports) {
  $text = (Get-Content -Path $report.FullName -Raw).Trim()
  # Defensive: drop a leading job-summary style heading if one is ever present.
  $text = ($text -replace '^\s*###\s+Benchmarks[^\r\n]*\r?\n', '').Trim()
  $text -split '\r?\n' -join $newline
}

$versionText = if ($Version) { " for v$Version" } else { '' }
$note = "_BenchmarkDotNet ShortRun results from the release build$versionText on a GitHub-hosted runner. Absolute timings vary between runs and machines; compare rows within a table._"

$updated = $head + $newline + $newline + $note + $newline + $newline + ($body -join ($newline + $newline)) + $newline
$changed = $updated -ne $original

if ($changed) {
  [System.IO.File]::WriteAllText($path, $updated, [System.Text.UTF8Encoding]::new($hasBom))
  Write-Host "Updated '$Heading' in $ReadmePath from $($reports.Count) report(s)."
}
else {
  Write-Host "$ReadmePath benchmarks are already up to date."
}

if ($env:GITHUB_OUTPUT) {
  "changed=$($changed.ToString().ToLowerInvariant())" | Add-Content -Path $env:GITHUB_OUTPUT
}

exit 0

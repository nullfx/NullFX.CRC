<#
.SYNOPSIS
  Verifies a packed NullFX.CRC NuGet package.

.DESCRIPTION
  Opens the .nupkg produced by `dotnet pack` and checks that:
    - the package id and version match what we expect
    - README.md is packed and referenced from the nuspec
    - the MIT license expression is present
    - every target framework in the library project has an assembly in lib/
    - no replace-tokens placeholders (#{...}#) survived
    - no test/benchmark assemblies or bin/obj paths were packed
  Writes a short summary to $env:GITHUB_STEP_SUMMARY when available.

.EXAMPLE
  ./.github/scripts/verify-package.ps1 -PackageDirectory ./artifacts/package -Version 0.0.0
#>
[CmdletBinding()]
param(
  [Parameter(Mandatory)] [string] $PackageDirectory,
  [Parameter(Mandatory)] [string] $Version,
  [string] $PackageId = 'NullFX.CRC',
  [string] $Project = 'NullFX.CRC/NullFX.CRC.csproj',
  # Optional override; by default the list is read from the project.
  [string[]] $TargetFrameworks
)

$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem

$failures = [System.Collections.Generic.List[string]]::new()
function Fail([string] $message) { $failures.Add($message); Write-Host "::error::$message" }

$nupkgPath = Join-Path $PackageDirectory "$PackageId.$Version.nupkg"
if (-not (Test-Path $nupkgPath)) {
  $found = (Get-ChildItem $PackageDirectory -Filter '*.nupkg' | Select-Object -ExpandProperty Name) -join ', '
  throw "Expected package '$nupkgPath' was not found. Found: $found"
}

# Target frameworks come from the project so this list never drifts.
if ($TargetFrameworks) {
  $expectedTfms = $TargetFrameworks
} else {
  $tfmRaw = (& dotnet msbuild $Project -getProperty:TargetFrameworks | Out-String).Trim()
  $expectedTfms = $tfmRaw.Split(';', [System.StringSplitOptions]::RemoveEmptyEntries)
}
if ($expectedTfms.Count -eq 0) { throw "Could not read TargetFrameworks from $Project" }

$zip = [System.IO.Compression.ZipFile]::OpenRead((Resolve-Path $nupkgPath))
try {
  $entries = $zip.Entries | ForEach-Object { $_.FullName }

  function Read-Entry([string] $name) {
    $entry = $zip.Entries | Where-Object { $_.FullName -eq $name } | Select-Object -First 1
    if (-not $entry) { return $null }
    $reader = [System.IO.StreamReader]::new($entry.Open())
    try { return $reader.ReadToEnd() } finally { $reader.Dispose() }
  }

  $nuspecName = $entries | Where-Object { $_ -like '*.nuspec' } | Select-Object -First 1
  if (-not $nuspecName) { throw 'No .nuspec found inside the package.' }
  [xml] $nuspec = Read-Entry $nuspecName
  $meta = $nuspec.package.metadata

  if ($meta.id -ne $PackageId)     { Fail "Package id is '$($meta.id)', expected '$PackageId'." }
  if ($meta.version -ne $Version)  { Fail "Package version is '$($meta.version)', expected '$Version'." }
  if ($meta.readme -ne 'README.md') { Fail "nuspec <readme> is '$($meta.readme)', expected 'README.md'." }
  if ($meta.license.'#text' -ne 'MIT') { Fail "nuspec license is '$($meta.license.'#text')', expected 'MIT'." }
  if ($entries -notcontains 'README.md') { Fail 'README.md is not packed at the package root.' }
  if ($meta.repository -and -not $meta.repository.commit) { Write-Host '::warning::Repository commit is missing from the nuspec (SourceLink).' }

  foreach ($tfm in $expectedTfms) {
    if ($entries -notcontains "lib/$tfm/NullFX.CRC.dll") { Fail "Missing lib/$tfm/NullFX.CRC.dll" }
  }

  foreach ($name in @($nuspecName, 'README.md')) {
    $text = Read-Entry $name
    if ($text -and $text -match '#\{[A-Z_]+\}#') { Fail "Unreplaced token found in $name." }
  }

  $unexpected = $entries | Where-Object {
    $_ -match '(?i)(^|/)(bin|obj)/' -or $_ -match '(?i)\.Tests\.dll$' -or $_ -match '(?i)Benchmarks' -or $_ -match '(?i)\.snk$'
  }
  foreach ($u in $unexpected) { Fail "Unexpected file in package: $u" }

  $summary = @(
    "### NuGet package: ``$PackageId $($meta.version)``",
    '',
    "| Check | Result |",
    "|---|---|",
    "| Version | ``$($meta.version)`` |",
    "| Target frameworks | $($expectedTfms -join ', ') |",
    "| README | $([bool]($entries -contains 'README.md')) |",
    "| License | $($meta.license.'#text') |",
    "| Files | $($entries.Count) |",
    "| Problems | $($failures.Count) |"
  ) -join "`n"
  Write-Host $summary
  if ($env:GITHUB_STEP_SUMMARY) { Add-Content -Path $env:GITHUB_STEP_SUMMARY -Value $summary }
}
finally {
  $zip.Dispose()
}

if ($failures.Count -gt 0) {
  throw "Package verification failed with $($failures.Count) problem(s)."
}
Write-Host "Package $nupkgPath verified."
exit 0

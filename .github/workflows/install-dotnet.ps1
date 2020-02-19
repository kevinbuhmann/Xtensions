Param (
  [Parameter(Mandatory=$true)]
  [ValidateSet("x86", "x64")]
  [string] $Architecture
)

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

. ".\.build\helper-functions.ps1"

$tempDirectory = [io.path]::GetFullPath("$PSScriptRoot\..\..\.tmp")
$dotnetDirectory = "$tempDirectory\dotnet-$Architecture"
$dotnetInstallFile = "$tempDirectory\dotnet-install.ps1"

mkdir -Force $tempDirectory > $null

mkdir -Force $tempDirectory > $null

$dotnetInstallUrl = "https://raw.githubusercontent.com/dotnet/cli/master/scripts/obtain/dotnet-install.ps1"
(New-Object System.Net.WebClient).DownloadFile($dotnetInstallUrl, $dotnetInstallFile)

Safe-Execute-Command { & $dotnetInstallFile -InstallDir $dotnetDirectory -Architecture $Architecture }

Write-Output "::add-path::$dotnetDirectory"

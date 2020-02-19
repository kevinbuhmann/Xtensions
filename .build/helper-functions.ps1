function Execute-Step($stepName, $step) {
  if ($env:GITHUB_WORKFLOW) {
    Write-Output "::group:: $stepName"
  } else {
    Write-Output ""
    Write-Output "╬═════════════════════"
    Write-Output "║ $stepName"
    Write-Output "╬════════════"
    Write-Output ""
  }

  Invoke-Command $step

  if ($env:GITHUB_WORKFLOW) {
    Write-Output "::endgroup::"
  }
}

function Safe-Execute-Command($command) {
  Invoke-Command $command

  if ($lastExitCode -ne 0) {
    exit $lastExitCode
  }
}

function Invoke-Dotnet($dotnetArgs) {
  Write-Host "> dotnet $dotnetArgs" -ForegroundColor DarkGray
  Safe-Execute-Command { Invoke-Expression "dotnet $dotnetArgs" }
}

function Normalize-Path($path) {
  return $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($path);
}

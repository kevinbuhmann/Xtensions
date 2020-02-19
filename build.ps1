Param (
  [int] $CoverageThreshold = 100
)

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

. ".\.build\helper-functions.ps1"

$configuration = "Release"
$artifactsPath = Normalize-Path("$PSScriptRoot\artifacts")

Execute-Step "Clean artifacts" {
  if (Test-Path $artifactsPath) {
    Remove-Item -Path $artifactsPath -Recurse
  }
}

Execute-Step "Install tools" {
  Invoke-Dotnet "tool restore"
}

Execute-Step "Build" {
  Invoke-Dotnet "build -c $configuration /t:rebuild"
}

foreach ($testProjectFile in Get-ChildItem -Path .\tests\**\*.Tests.csproj) {
  $testProject = $testProjectFile.Name.Replace(".csproj", "")
  $projectUnderTest = $testProject.Replace(".Tests", "")
  $testAssemblyPath = Normalize-Path("$($testProjectFile.Directory.FullName)\bin\$configuration\netcoreapp3.1\$testProject.dll")
  $outputPath = Normalize-Path("$artifactsPath\coverage\$testProject.xml")

  Execute-Step "Test $projectUnderTest" {
    $coverletArgs = $testAssemblyPath,
                    "--target dotnet",
                    "--targetargs `"test -c $configuration $testProjectFile --no-build`"",
                    "--include `"[$projectUnderTest]*`"",
                    "--threshold $CoverageThreshold",
                    "--output $outputPath",
                    "--format opencover"

    Invoke-Dotnet "tool run coverlet $($coverletArgs -Join ' ')"
  }
}

Execute-Step "Generate code coverage report" {
  $reports = Normalize-Path("$artifactsPath\coverage\*.xml")
  $reportPath = Normalize-Path("$artifactsPath\coverage\report")

  $reportGeneratorArgs = "--reports:$reports",
                         "--targetdir:$reportPath",
                         "--reporttypes:html"

  Invoke-Dotnet "tool run reportgenerator $($reportGeneratorArgs -Join ' ')"
}

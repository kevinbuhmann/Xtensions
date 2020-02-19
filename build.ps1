Param (
  [int] $CoverageThreshold = 100
)

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

. ".\.build\helper-functions.ps1"

$configuration = "Release"
$artifactsPath = "$PSScriptRoot\artifacts"

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
  $testAssemblyPath = "$($testProjectFile.Directory.FullName)\bin\$configuration\netcoreapp3.1\$testProject.dll"
  $outputPath = "$artifactsPath\coverage\$testProject.xml"

  Execute-Step "Test $projectUnderTest" {
    $coverletArguments = $testAssemblyPath,
      "--target dotnet",
      "--targetargs `"test -c $configuration $testProjectFile --no-build`"",
      "--include `"[$projectUnderTest]*`"",
      "--threshold $CoverageThreshold",
      "--output $outputPath",
      "--format opencover"

    $coverletArgumentsString = $coverletArguments -Join ' '

    Invoke-Dotnet "tool run coverlet $coverletArgumentsString"
  }
}

Execute-Step "Generate code coverage report" {
  Invoke-Dotnet "tool run reportgenerator --reports:$artifactsPath\coverage\*.xml --targetdir:$artifactsPath\coverage\report --reporttypes:html"
}

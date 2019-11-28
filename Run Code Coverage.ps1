
Write-Host "Check the report generator is installed"

$dotnetToolOutput = dotnet tool list -g | Select-String -pattern "dotnet-reportgenerator-globaltool" -SimpleMatch

if ([string]::IsNullOrEmpty($dotnetToolOutput)) {
    Write-Warning "Report generator could not be found"
    dotnet tool install -g dotnet-reportgenerator-globaltool
}
else {
    Write-Host "Report generator is already installed"
}

Write-Host ""
Write-Host "Run code coverage tests"
Write-Host "----- Code Coverage --------"

& ".\Launch Integration Environment.ps1"

dotnet test "tests\NHSD.BuyingCatalogue.Infrastructure.Tests\NHSD.BuyingCatalogue.Infrastructure.Tests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]*
dotnet test "tests\NHSD.BuyingCatalogue.API.UnitTests\NHSD.BuyingCatalogue.API.UnitTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.Capabilities.API.UnitTests\NHSD.BuyingCatalogue.Capabilities.API.UnitTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.Capabilities.Application.UnitTests\NHSD.BuyingCatalogue.Capabilities.Application.UnitTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.Capabilities.Persistence.DatabaseTests\NHSD.BuyingCatalogue.Capabilities.Persistence.DatabaseTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.SolutionLists.API.UnitTests\NHSD.BuyingCatalogue.SolutionLists.API.UnitTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.SolutionLists.Application.UnitTests\NHSD.BuyingCatalogue.SolutionLists.Application.UnitTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.SolutionLists.Persistence.DatabaseTests\NHSD.BuyingCatalogue.SolutionLists.Persistence.DatabaseTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.Solutions.API.UnitTests\NHSD.BuyingCatalogue.Solutions.API.UnitTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.Solutions.Application.UnitTests\NHSD.BuyingCatalogue.Solutions.Application.UnitTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests\NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.Data.Tests\NHSD.BuyingCatalogue.Data.Tests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json"
dotnet test "tests\NHSD.BuyingCatalogue.Persistence.DatabaseTests\NHSD.BuyingCatalogue.Persistence.DatabaseTests.csproj" --no-build --nologo /p:CollectCoverage=true /p:CoverletOutput=../../OpenCover/ /p:Exclude=[*.Testing.*]* /p:MergeWith="../../OpenCover/coverage.json" /p:CoverletOutputFormat="opencover"

Write-Host "----- Code Coverage --------"
Write-Host ""
Write-Host "Generate code coverage reports"
Write-Host "----- Report Output--------"

reportgenerator "-reports:OpenCover\coverage.opencover.xml" "-targetdir:OpenCover\Report\"

Write-Host "----- Report Output--------"

& ".\Tear Down Integration Environment.ps1"
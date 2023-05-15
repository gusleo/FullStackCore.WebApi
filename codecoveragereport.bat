@echo off

set OpenCoverPath="C:\Path\To\OpenCover\OpenCover.Console.exe"
set ReportGeneratorPath="C:\Path\To\ReportGenerator\ReportGenerator.exe"

set CoverageReportDir=".\CoverageReports"
set HTMLReportDir=".\HTMLReports"

rem Create coverage report and HTML report directories if they don't exist
if not exist "%CoverageReportDir%" mkdir "%CoverageReportDir%"
if not exist "%HTMLReportDir%" mkdir "%HTMLReportDir%"

rem Iterate through test projects with "Test" suffix
for /R %%I in (*.Test.dll) do (
    set TestProject=%%~fI
    set CoverageReport="%CoverageReportDir%\%%~nI_CoverageReport.xml"
    set HTMLReportDir="%HTMLReportDir%\%%~nI_HTMLReport"

    rem Run tests and generate coverage report for the current test project
    %OpenCoverPath% -target:"nunit3-console" -targetargs:"%TestProject%" -output:"%CoverageReport%"
    %ReportGeneratorPath% -reports:"%CoverageReport%" -targetdir:"%HTMLReportDir%"
)

echo Code coverage reports have been generated.

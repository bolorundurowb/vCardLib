nuget install NUnit.Runners -Version 3.4.1 -OutputDirectory tools
nuget install OpenCover -Version 4.6.519 -OutputDirectory tools
nuget install coveralls.net -Version 0.7.0 -OutputDirectory tools
nuget install ReportGenerator -Version 2.4.5.0 -OutputDirectory tools

cd C:\projects\vcf-reader\

.\tools\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user "-filter:+[vCard*]*" "-target:.\tools\NUnit.ConsoleRunner.3.4.1\tools\nunit3-console.exe" "-targetargs: .\vCardLib.Tests\bin\Debug\vCardLib.Tests.dll"

.\tools\ReportGenerator.2.4.5.0\ReportGenerator.exe "-reports:results.xml" "-targetdir:.\coverage"

.\tools\csmacnz.Coveralls.exe --opencover -i .\TestResult.xml

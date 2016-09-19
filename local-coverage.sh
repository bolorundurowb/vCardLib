mono ./packages/NUnit.ConsoleRunner.3.4.1/tools/nunit3-console.exe ./vCardLib.Tests/bin/Debug/vCardLib.Tests.dll

mono ./packages/OpenCover.4.6.519/tools/OpenCover.Console.exe -register:user -target:./packages/NUnit.ConsoleRunner.3.4.1/tools/nunit3-console.exe -targetargs:./vCardLib.Tests/bin\Debug/vCardLib.Tests.dll

mono ./packages/ReportGenerator.2.4.5.0/tools/ReportGenerator.exe -reports:TestResult.xml -targetdir:./coverage

mono ./packages/coveralls.net.0.7.0/tools/csmacnz.Coveralls.exe --opencover -i ./TestResult.xml
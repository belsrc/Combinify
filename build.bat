REM Build the project exe
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombinifyWpf.sln" /property:Configuration=Release

REM Run the unit tests and generate the coverage file
"E:\Programming Enviroments\OpenCover\OpenCover.Console.exe" -register:user "-target:E:\Programming Enviroments\NUnit 2.6.2\bin\nunit-console.exe" -targetargs:"/fixture:CombiTests \"E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiTests\CombiTests\bin\Debug\CombiTests.dll\" /noshadow" "-output:E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\Test\TestResults.xml" -filter:"+[*]*"

REM Generate the coverage report
"E:\Programming Enviroments\ReportGenerator_1.8.0.0\ReportGenerator.exe" "E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\Test\TestResults.xml" "E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\CoverageResult"

REM Generate SandCastle Documentation
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\CombiDocs.shfbproj"

REM Clean up
COPY "E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\report.css" "E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\CoverageResult"
COPY "E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\Presentation.css" "E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\Help\styles"
COPY "E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\TOC.css" "E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\Help"
DEL "E:\Programming\!Csharp\Combinify Project\CombinifyWpf\TestResult.xml"

REM Open the generate docs
"E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\CoverageResult\index.htm"
"E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombiDocs\Help\Index.html"
exit
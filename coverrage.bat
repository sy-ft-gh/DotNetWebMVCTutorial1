REM ###### Settings ######

SET PROJECT_NAME=WebTutorial1
SET TEST_PROJECT_NAME=UnitTestProject1

SET MS_TEST=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\MSTest.exe

SET REPORT_NAME=result.xml
SET OUTPUT_DIR=.\html

SET OPEN_COVER=.\packages\OpenCover.4.7.922\tools\OpenCover.Console.exe
SET REPORT_GEN=.\packages\ReportGenerator.4.8.6\tools\netcoreapp3.0\ReportGenerator.exe

SET TEST=.\%TEST_PROJECT_NAME%\bin\Debug\%TEST_PROJECT_NAME%.dll
SET COVERAGE_DIR=.\%PROJECT_NAME%\bin\Debug\
SET FILTERS=+[%PROJECT_NAME%]*

REM #######################

call :EXECUTE "%TEST%"
start "" %OUTPUT_DIR%\index.htm

exit

:EXECUTE

%OPEN_COVER% -register:user -target:"%MS_TEST%" -targetargs:"/noisolation /testcontainer:\"%~f1\"" -targetdir:%COVERAGE_DIR% -filter:"%FILTERS%" -output:%REPORT_NAME% -mergebyhash
%REPORT_GEN% "%REPORT_NAME%" %OUTPUT_DIR%

exit /b
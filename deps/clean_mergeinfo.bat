@echo off

set script_folder=%~dp0
set TEMPFILE=%TEMP%\clean_mergeinfo.tmp

if exist %TEMPFILE% del /f /q %TEMPFILE%
for /f "usebackq delims=: tokens=1,2" %%a in (`svn propget svn:mergeinfo %script_folder%`) do (
	if not "%%a" == "/trunk" (
		echo %%a:%%b>> %TEMPFILE%
	)
)
pause

call svn propset svn:mergeinfo -F %TEMPFILE% %script_folder%

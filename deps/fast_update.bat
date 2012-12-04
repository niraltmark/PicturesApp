@echo off

%~d0
cd %~dp0

set batch_path=%~dp0
set SVN_COMMAND=%batch_path%\net\build\SlikSvn\svn
rem set SVN_COMMAND=c:\utils\SlikSvn\svn
set tmp_log=%batch_path%\fast_svn1.log
set tmp_log2=%batch_path%\fast_svn2.log
@echo off > %tmp_log%
@echo off > %tmp_log2%

FOR /F "usebackq tokens=2" %%A in (`%SVN_COMMAND% log -v -q -r HEAD:BASE %batch_path% ^| find "/"`) do (
	FOR /F "usebackq tokens=1,2,* delims=/" %%B in ('%%A') do (
		if not "%%B" == "" (
			if "%%B" == "trunk" (
				if not "%%C" == "" (
					if not "%%D" == "" (
						echo %%C/%%D >> %tmp_log%
					) else (
						echo %%C >> %tmp_log%
					)
				)
			) else (
				if not "%%D" == "" (
					echo %%D >> %tmp_log%
				)
			)
		)
	)
)

echo ================== >> %tmp_log%
setLocal EnableDelayedExpansion
for /f "usebackq tokens=* delims= " %%A in (`type %tmp_log%`) do (
	set ITEMDIR=%%~dpA
	find "!ITEMDIR!$" < %tmp_log2% > nul
	if errorlevel 1 (
		if not exist !ITEMDIR! (
			echo WARNING: !ITEMDIR! DOES NOT EXIST. LOOK FOR PARENT
			set ITEMDIR=!ITEMDIR:\= !
			set PARENT=
			for %%B in (!ITEMDIR!) do (
				if "!PARENT!" == "" (
					if exist "%%B" set PARENT=%%B
				) else (
					if exist "!PARENT!\%%B" set PARENT=!PARENT!\%%B
				)
			)
			echo !PARENT!$>> %tmp_log2%
			echo update --depth infinity !PARENT!
			%SVN_COMMAND% up !PARENT!
		)
	)
)
for /f "usebackq tokens=* delims= " %%A in (`type %tmp_log%`) do (
	set ITEMDIR=%%~dpA
	find "!ITEMDIR!$" < %tmp_log2% > nul
	if errorlevel 1 (
		if exist !ITEMDIR! (
			echo !ITEMDIR!$>> %tmp_log2%
			echo update !ITEMDIR!
			%SVN_COMMAND% up --depth=immediates !ITEMDIR!
		)
	)
)
endlocal

echo update %batch_path%
%SVN_COMMAND% up --depth=files %batch_path%

pause



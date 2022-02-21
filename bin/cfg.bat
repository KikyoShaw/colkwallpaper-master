echo off
set root_dir=%~dp0

xcopy /y /s /r /f  /i "%root_dir%\Clien\*" "%root_dir%\Debug\netcoreapp3.1\"
xcopy /y /s /r /f  /i "%root_dir%\Clien\*" "%root_dir%\Release\netcoreapp3.1\"

@echo off
set path1=%1
set path2=%2

FORFILES /p %1 /M *.cs /c "cmd /c copy \"%1\@file\" \"%2\@file\" /Y"
::if errorlevel 1 @exit 0

echo "pre-building..."
set OLDDIR=%CD%
set DESTDIR=%1
dir ..\webapp
yarn build
echo "web built"
dir ..
rmdir /s /q %DESTDIR%
robocopy webapp\dist %DESTDIR% /E
chdir /d %OLDDIR% &rem restore current directory

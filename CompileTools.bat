dotnet publish -c Release Preview.UI
"D:\Windows Kits\10\bin\10.0.22621.0\x64\signtool.exe" sign /n "bnszs" /fd SHA256 /t http://timestamp.digicert.com "Preview.UI\bin\Preview.UI.exe"
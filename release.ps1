@echo off
dotnet publish -c Release
$ExePath = "bin\Release\netcoreapp3.1\win10-x64\publish\central.exe"
C:\tools\ResourceHacker\ResourceHacker.exe -open $ExePath -save $ExePath -action addskip -res icon.ico -mask ICONGROUP,MAINICO

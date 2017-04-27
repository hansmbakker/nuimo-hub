param ([string]$ip, [string]$destination)

& ".\publish-windows.ps1"

#& xcopy.exe /y /e /d ".\bin\Debug\netcoreapp2.0\win8-arm\publish" "\\$ip\$destination"
& robocopy.exe /MIR ".\bin\Debug\netcoreapp2.0\win8-arm\publish" "\\$ip\$destination"
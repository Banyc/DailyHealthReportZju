$Rid = "linux-x64"
# $Rid = "linux-arm64"
# $Rid = "linux-arm"
# $Rid = "win-x64"
# $Rid = "win-x86"

Remove-Item ./DailyHealthReportZju."$Rid".zip
Set-Location .\src
Set-Location .\DailyHealthReportZju
dotnet publish -r "$Rid" --no-self-contained
Set-Location ..

# zip
7z.exe a -tzip DailyHealthReportZju."$Rid".zip ./DailyHealthReportZju/bin/Debug/netcoreapp3.1/"$Rid"/publish/*
Move-Item ./DailyHealthReportZju."$Rid".zip ..
Set-Location ..
dotnet restore

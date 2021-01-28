# $Rid = "linux-x64"
# $Rid = "linux-arm64"
$Rid = "linux-arm"
# $Rid = "win-x64"
# $Rid = "win-x86"
$Project = "DailyHealthReportZju"
$ShortName = $Project
$ProjectDirectoryPath = "./src"

$ProjectPath = Join-Path -Path $ProjectDirectoryPath -ChildPath $Project

Remove-Item ./$ShortName.$Rid.zip
dotnet publish $ProjectPath -r $Rid --no-self-contained

# zip
Compress-Archive -Path $ProjectPath/bin/Debug/net5.0/$Rid/publish/* -DestinationPath ./$ShortName.$Rid.zip

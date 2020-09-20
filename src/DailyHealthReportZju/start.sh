#!/bin/sh
cd "$(dirname "$0")"
cd src
cd DailyHealthReportZju
dotnet restore
dotnet run

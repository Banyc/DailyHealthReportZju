#!/bin/bash

# RID="linux-x64"
# RID="linux-arm64"
RID="linux-arm"
# RID="win-x64"
# RID="win-x86"
PROJECT="DailyHealthReportZju"
SHORT_NAME="DailyHealthReportZju"

mkdir $HOME/opt/$PROJECT
unzip ./$SHORT_NAME.$RID.zip -d $HOME/opt/$PROJECT
chmod +x $HOME/opt/$PROJECT/$PROJECT

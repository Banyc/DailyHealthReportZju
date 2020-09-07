# DailyHealthReportZju

Using Selenium to auto submit daily health report

## Disclaimer

We (as the creator of this tool) has NO responsibility for any damages you suffer as a result of using our products or services.

YOU TAKE YOUR OWN RISK BY USING OUR PRODUCT.

## Build

```shell
cd src/DailyHealthReportZju
dotnet restore
dotnet run
```

If this is the first time you run, please follow the instruction in "Customize" Section below. Do NOT set field `IsFullAutoMode` to be true.

After that:

- login with your account.
    - Then, close the browser and run the program again.
- OR set your username and password in file `appsettings`

And then please follow the instruction in console to complete your geometric information.

## Customize

1. Go to file `appsettings` and set field `IsFullAutoMode` to be false;
1. check every item in that file;
1. Run the program and check if everything goes as expected;
1. If you want a full automation, go to `appsettings` and set field `IsFullAutoMode` to be true;

## TODO

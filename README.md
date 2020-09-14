# DailyHealthReportZju

Using Selenium to auto submit daily health report

## Disclaimer

We (as the creator of this tool) has NO responsibility for any damages you suffer as a result of using our products or services.

YOU TAKE YOUR OWN RISK BY USING OUR PRODUCT.

## Prerequisite

- [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1) to run C# code in three main platform
- [Chrome](https://www.google.com/intl/en-us/chrome/)
- The [Chrome driver](https://chromedriver.chromium.org/downloads) that matches the version of Chrome

## Run

1. Download and install (except the Chrome driver) those prerequisites above;
1. Customize the configuration file [appsettings](./src/DailyHealthReportZju/appsettings.json);
1. Start a command line promote at the root of the project and type in the following lines:

    ```shell
    cd src/DailyHealthReportZju
    dotnet restore
    dotnet run

    ```

### Caution

If this is the first time you run, please follow the instruction in "Customize" Section below. Do NOT set field `IsFullAutoMode` to be true.

After that:

- run the APP and login with your account in the Chrome pop-up manually.
    - Then, close the browser and run the program again.
- OR set your `Username` and `Password` in file `appsettings` before running the APP

And then please follow the instruction in console to complete your geometric information.

## Customize

1. Go to file `appsettings` and set field `IsFullAutoMode` to be false;
1. check every item in that file;
    - check if every item in `KeyWords` meets your need
    - check if `PathToChromeDriver` is pointing to the Chrome driver you have downloaded
    - set up `Username` and `Password` fields
        - if you don't trust this APP, you could either:
            - audit the source code of the program;
            - or leave blank and login manually every time the cookies are expired.
1. Run the program and check if everything goes as expected;
1. If you want a full automation, go to `appsettings` and set field `IsFullAutoMode` to be `true`;
1. If you are annoyed by the pop-up of the Chrome browser, set `IsHeadless` to be `true`;

## TODO

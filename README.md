# DailyHealthReportZju

Using Selenium to submit daily health reports.

## Disclaimer

We (as the creator of this tool) has NO responsibility for any damages you suffer as a result of using our products or services.

YOU TAKE YOUR OWN RISK BY USING OUR PRODUCT.

## Prerequisite

- [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet/5.0) to run C# code in Linux/MacOs/Windows
    - 这里多说一句：现在已经是 **0202** 年了，`.NET Framework` 不等于 `.NET Core`，后者是完美支持跨平台，`C#` 语言【支持】跨平台
- [Chrome](https://www.google.com/intl/en-us/chrome/)
- The [Chrome driver](https://chromedriver.chromium.org/downloads) that matches the version of Chrome

## Download Links

Go to the Action page of this repository for the latest binaries.

## Run from Binary

1.  Permit the binary.
1.  Run the binary.

## Run from Source code

### Windows/MacOS

1. Download and install (except the Chrome driver) those prerequisites above;
1. Customize the configuration file [appsettings](./src/DailyHealthReportZju/appsettings.json);
1. Start a command line prompt at the root of the project and type in the following lines:

    ```shell
    cd src/DailyHealthReportZju
    dotnet restore
    dotnet run

    ```

### Ubuntu/Debian

1. run `sudo apt install chromium-chromedriver`
1. Customize the configuration file [appsettings](./src/DailyHealthReportZju/appsettings.json)
    - and setting the field `PathToChromeDriver` with `/usr/lib/chromium-browser`
1. Start a command line prompt at the root of the project and type in the following lines:

    ```shell
    cd src/DailyHealthReportZju
    dotnet restore
    dotnet run

    ```

### Caution

If this is the first time you run, please follow the instruction in the "Customize" Section below. Do NOT set the field `IsFullAutoMode` to be true.

After that:

- run the APP and login with your account in the Chrome pop-up manually.
    - Then, close the browser and run the program again.
- OR set your `Username` and `Password` in file `appsettings.secret.json` before running the APP

And then, please follow the instruction in the console to complete your geometric information.

## Customize

1. Make a copy of `appsettings.json` and name it `appsettings.secret.json`;
1. Go to file `appsettings.secret.json` and set field `IsFullAutoMode` to be false;
1. check every item in that file;
    - check if every item in `KeyWords` meets your need
    - check if `PathToChromeDriver` is pointing to the Chrome driver you have downloaded
    - set up `Username` and `Password` fields
        - if you don't trust this APP, you could either:
            - audit the source code of the program;
            - or leave blank and log in manually every time the cookies are expired.
1. Run the program and check if everything goes as expected;
1. If you want full automation, go to `appsettings.secret.json` and set the field `IsFullAutoMode` to be `true`;
1. If you are annoyed by the pop-up of the Chrome browser, set `IsHeadless` to be `true`;

## TODO

- [x] Make a hosted and scheduled service for Linux server
<!-- - [ ] Multiple users -->

using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace DailyHealthReportZju.Services
{
    // only works on <https://healthreport.zju.edu.cn/ncov/wap/default/index>
    public partial class HealthReportZju
    {
        private IWebDriver GetDriver(string url)
        {
            IWebDriver driver = null;
            switch (_config.DriverTypes)
            {
                case DriverTypes.Chrome:
                    // options.AddArguments(@"user-data-dir=%userprofile%\AppData\Local\Google\Chrome\User Data\NAMEYOUCHOOSE");

                    ChromeOptions options = new ChromeOptions();
                    if (_config.IsHeadless)
                    {
                        options.AddArguments("--headless");
                        options.AddArguments("start-maximized");
                        options.AddArguments("--disable-gpu");
                        options.AddArguments("--no-sandbox");
                        // options.AcceptInsecureCertificates = true;
                    }
                    else
                    {
                        // specify location for profile creation/ access
                        options.AddArguments("--user-data-dir=./userData.chrome");
                        options.AddArguments("--window-size=1,200");
                    }
                    options.AddArguments("--disable-extensions");
                    options.AddArguments("--disable-geolocation");

                    // options.AddAdditionalCapability("pageLoadStrategy", "none");
                    driver = new ChromeDriver(".", options);

                    // hide the browser window
                    if (_config.IsFullAutoMode && !_config.IsHeadless)
                    {
                        driver.Manage().Window.Position = new System.Drawing.Point(-10000, 0);
                    }
                    break;
                case DriverTypes.Firefox:
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    // firefoxOptions.AddArguments("--headless");
                    // firefoxOptions.AddArguments("--start-maximized");
                    // firefoxOptions.AddArguments("--window-size=1920,1080");
                    // firefoxOptions.AddArguments("-width=1920");
                    // firefoxOptions.AddArguments("-height=1080");

                    // FirefoxProfile profile = new FirefoxProfile(@"./profile.firefox");
                    // ProfilesIni allProfiles = new ProfilesIni();
                    // FirefoxProfile desiredProfile = allProfiles.getProfile("SELENIUM");
                    // var profileManager = new FirefoxProfileManager();
                    // FirefoxProfile profile = profileManager.GetProfile("SeleniumUser");

                    // var allProfiles = new FirefoxProfileManager();
                    // if (!allProfiles.ExistingProfiles.Contains("SeleniumUser"))
                    // {
                    //     throw new Exception("SeleniumUser firefox profile does not exist, please create it first.");
                    // }
                    // var profile = allProfiles.GetProfile("SeleniumUser");
                    // profile.SetPreference("webdriver.firefox.profile", "SeleniumUser");

                    // firefoxOptions.Profile = profile;

                    driver = new FirefoxDriver(".", firefoxOptions);
                    break;
                default:
                    break;
            }
            // tolaration before an element is available for detection
            if (_config.DriverTypes == DriverTypes.Chrome)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_config.InitiationTimeoutInSeconds);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(_config.InitiationTimeoutInSeconds);
            }

            bool isLoadedSuccessful = false;
            while (!isLoadedSuccessful)
            {
                try
                {
                    driver.Navigate().GoToUrl(url);
                }
                catch (WebDriverException)
                {
                    // progress even if the page is not fully loaded
                }

                try
                {
                    driver.FindElement(By.TagName("body"));
                    isLoadedSuccessful = true;
                }
                catch (NoSuchElementException)
                {

                }
            }
            if (_config.DriverTypes == DriverTypes.Chrome)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_config.ElementDiscoveryTimeoutInSeconds);
            }

            return driver;
        }
    }
}

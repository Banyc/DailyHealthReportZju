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
    public class HealthReportZju
    {
        private readonly Helpers.ChangeDetector<string> _changeDetector = new Helpers.ChangeDetector<string>();

        public void Start()
        {
            // SemaphoreSlim videoStableSignal = new SemaphoreSlim(0, 1);

            // Console.WriteLine("[Info] This program could only handle one video once.");
            // Console.WriteLine("[Info] It won't automatically go to the next video.");
            // Console.WriteLine("[Info] It won't refresh the website when video is not playing either.");
            Console.WriteLine("[Info] [Disclaimer] We (as the creator of this tool) has NO responsibility for any damages you suffer as a result of using our products or services");

            IWebDriver driver;
            try
            {
                driver = GetDriver(GlobalSettings.Url);
                // driver.Manage().Window.Minimize();  // This will disable the whole auto process
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR] Please close the previous chrome popup then restart this program.");
                Console.WriteLine("[ERROR] If problem persists, download chromedriver of corresponding version (link below) and replace the old one.");
                Console.WriteLine("[ERROR] <https://chromedriver.chromium.org/downloads>");
                return;
            }

            // List<string> selectors = GlobalSettings.GetSelectors();
            List<(string, string)> keyWords = GlobalSettings.GetKeyWords();

            // click all shits
            // CheckAllOptions(driver, selectors);
            CheckAllOptions(driver, keyWords);

            // select gep position
            SetGeo(driver);

            if (GlobalSettings.IsFullAutoMode)
            {
                // submit
                Submit(driver);

                // process about to exit
                Console.WriteLine("[Info] End of the the function `Start`");
                driver.Quit();
            }
        }

        private void CheckAllOptions(IWebDriver driver, List<string> selectors)
        {
            foreach (string selector in selectors)
            {
                try
                {
                    IWebElement check = driver.FindElement(By.CssSelector(selector));
                    check.Click();
                }
                catch (StaleElementReferenceException)
                {
                    IWebElement check = driver.FindElement(By.CssSelector(selector));
                    check.Click();
                }
            }
        }
        private void CheckAllOptions(IWebDriver driver, List<(string, string)> keyWords)
        {
            foreach ((string preText, string keyword) in keyWords)
            {
                // bug: found but not checkbox not checked
                // string xpath = $"//*/text()[contains(.,'{preText}')]/following::div[contains(.,'{keyword}') and not(div)]";
                // workaround
                string xpath = $"//*/text()[contains(.,'{preText}')]/following::span[contains(.,'{keyword}') and not(span)]";
                try
                {
                    IWebElement check = driver.FindElement(By.XPath(xpath));
                    check.Click();
                }
                catch (StaleElementReferenceException)
                {
                    IWebElement check = driver.FindElement(By.XPath(xpath));
                    check.Click();
                }
            }
        }

        private List<int> GetGeoInfo()
        {
            string filename = "geoIndex.txt";
            string inputStr;
            if (File.Exists(filename))
            {
                // read indexes
                using (FileStream fs = File.OpenRead(filename))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        inputStr = sr.ReadLine();
                    }
                }
            }
            else
            {
                Console.WriteLine("[Instruction] Input your geo location indexes here (ex. \"0 2 3\")");
                Console.WriteLine("[Instruction] Explanation: those indexes are respectively your province/city/region");
                Console.WriteLine("[Instruction] Explanation: \"1 1 2\" represents Beijing/Beijing/Xicheng");
                Console.Write("> ");
                inputStr = Console.ReadLine();
                // save indexes
                using (File.Create(filename)) { }
                using (FileStream fs = File.OpenWrite(filename))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(inputStr);
                    }
                }
            }
            string[] locationIndexesStr = inputStr.Split();
            List<int> locationIndexes = new List<int>();
            foreach (var indexStr in locationIndexesStr)
                locationIndexes.Add(int.Parse(indexStr));
            return locationIndexes;
        }

        private void SetGeo(IWebDriver driver)
        {
            if (GlobalSettings.DriverTypes == DriverTypes.Chrome)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            }
            string selector = "body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(22) > div > input[type=text]";
            IWebElement check = driver.FindElement(By.CssSelector(selector));
            check.Click();
            try
            {
                // click affirmative btn
                selector = "#wapat > div > div.wapat-btn-box > div";
                IWebElement affirm = driver.FindElement(By.CssSelector(selector));
                affirm.Click();

                // get geo info
                List<int> locationIndexes = GetGeoInfo();

                // expand options list
                selector = "body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(23) > div > div > select.hcqbtn.hcqbtn-danger";
                IWebElement provinceRoot = driver.FindElement(By.CssSelector(selector));
                provinceRoot.Click();
                // select province
                selector = $"body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(23) > div > div > select.hcqbtn.hcqbtn-danger > option:nth-child({locationIndexes[0]})";
                IWebElement province = driver.FindElement(By.CssSelector(selector));
                province.Click();
                // select options list
                selector = "body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(23) > div > div > select.hcqbtn.hcqbtn-warning";
                IWebElement cityRoot = driver.FindElement(By.CssSelector(selector));
                cityRoot.Click();
                // select city
                selector = $"body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(23) > div > div > select.hcqbtn.hcqbtn-warning > option:nth-child({locationIndexes[1]})";
                IWebElement city = driver.FindElement(By.CssSelector(selector));
                city.Click();
                // select options list
                selector = "body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(23) > div > div > select.hcqbtn.hcqbtn-primary";
                IWebElement localRoot = driver.FindElement(By.CssSelector(selector));
                localRoot.Click();
                // select local
                selector = $"body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(23) > div > div > select.hcqbtn.hcqbtn-primary > option:nth-child({locationIndexes[2]})";
                IWebElement local = driver.FindElement(By.CssSelector(selector));
                local.Click();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("[Info] Geometry seems to be succeeded");
            }
            finally
            {
                if (GlobalSettings.DriverTypes == DriverTypes.Chrome)
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(GlobalSettings.ElementDiscoveryTimeoutInSeconds);
                }
            }
        }

        private void Submit(IWebDriver driver)
        {
            // submit
            string selector = "body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.list-box > div > a";
            IWebElement submit = driver.FindElement(By.CssSelector(selector));
            submit.Click();
            // confirm
            selector = "#wapcf > div > div.wapcf-btn-box > div.wapcf-btn.wapcf-btn-ok";
            IWebElement confirm = driver.FindElement(By.CssSelector(selector));
            confirm.Click();
        }

        private IWebDriver GetDriver(string url)
        {
            IWebDriver driver = null;
            switch (GlobalSettings.DriverTypes)
            {
                case DriverTypes.Chrome:
                    // options.AddArguments(@"user-data-dir=%userprofile%\AppData\Local\Google\Chrome\User Data\NAMEYOUCHOOSE");

                    ChromeOptions options = new ChromeOptions();
                    // specify location for profile creation/ access
                    options.AddArguments("--user-data-dir=./userData.chrome");
                    // options.AddArguments("--headless");
                    // options.AddArguments("start-maximized");
                    // options.AddArguments("--disable-gpu");
                    options.AddArguments("--window-size=1,200");
                    options.AddArguments("--disable-extensions");
                    options.AddArguments("--disable-geolocation");

                    // options.AddAdditionalCapability("pageLoadStrategy", "none");
                    driver = new ChromeDriver(".", options);
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
            if (GlobalSettings.DriverTypes == DriverTypes.Chrome)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(GlobalSettings.InitiationTimeoutInSeconds);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(GlobalSettings.InitiationTimeoutInSeconds);
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
            if (GlobalSettings.DriverTypes == DriverTypes.Chrome)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(GlobalSettings.ElementDiscoveryTimeoutInSeconds);
            }

            return driver;
        }
    }
}

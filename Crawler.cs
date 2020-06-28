using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace DailyHealthReportZju
{
    // only works on <https://healthreport.zju.edu.cn/ncov/wap/default/index>
    public class HealthReportZju
    {
        private readonly Helpers.ChangeDetector<string> _changeDetector = new Helpers.ChangeDetector<string>();

        public void Start()
        {
            string filename = "geoIndex.txt";
            SemaphoreSlim videoStableSignal = new SemaphoreSlim(0, 1);

            // Console.WriteLine("[Info] This program could only handle one video once.");
            // Console.WriteLine("[Info] It won't automatically go to the next video.");
            // Console.WriteLine("[Info] It won't refresh the website when video is not playing either.");
            Console.WriteLine("[Info] [Disclaimer] We (as the creator of this tool) has NO responsibility for any damages you suffer as a result of using our products or services");
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
                using (File.Create(filename)){}
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

            string url = "https://healthreport.zju.edu.cn/ncov/wap/default/index";

            IWebDriver driver;
            try
            {
                driver = GetDriver(url);
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
            SetGeo(driver, locationIndexes);

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
                string xpath = $"//*/text()[contains(.,'{preText}')]/following::div[contains(.,'{keyword}') and not(div)]";
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

        private void SetGeo(IWebDriver driver, List<int> locationIndexes)
        {
            string selector = "body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(22) > div > input[type=text]";
            IWebElement check = driver.FindElement(By.CssSelector(selector));
            check.Click();
            // click affirmative btn
            selector = "#wapat > div > div.wapat-btn-box > div";
            IWebElement affirm = driver.FindElement(By.CssSelector(selector));
            affirm.Click();
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
            // options.AddArguments(@"user-data-dir=%userprofile%\AppData\Local\Google\Chrome\User Data\NAMEYOUCHOOSE");

            ChromeOptions options = new ChromeOptions();
            // specify location for profile creation/ access
            options.AddArguments(@"user-data-dir=./userData");
            IWebDriver driver = new ChromeDriver(".", options);
            // tolaration before an element is available for detection
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl(url);
            return driver;
        }
    }
}

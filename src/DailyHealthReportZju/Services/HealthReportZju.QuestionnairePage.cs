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
using Microsoft.Extensions.Logging;
using DailyHealthReportZju.Models;

namespace DailyHealthReportZju.Services
{
    // only works on <https://healthreport.zju.edu.cn/ncov/wap/default/index>
    public partial class HealthReportZju
    {
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
        private void CheckAllOptions(IWebDriver driver, List<KeyValuePairString> keyWords)
        {
            foreach (KeyValuePairString keyValuePair in keyWords)
            {
                try
                {
                    IWebElement check = GetWebElement(driver, keyValuePair);
                    if (check.Displayed)
                    {
                        check.Click();
                    }
                }
                catch (StaleElementReferenceException)
                {
                    IWebElement check = GetWebElement(driver, keyValuePair);
                    if (check.Displayed)
                    {
                        check.Click();
                    }
                }
            }
        }

        private IWebElement GetWebElement(IWebDriver driver, KeyValuePairString keyValuePair)
        {
            // bug: checkbox found but not checked when clicking div
            // string xpath = $"//*/text()[contains(.,'{keyValuePair.Key}')]/following::div[contains(.,'{keyValuePair.Value}') and not(div)]";
            // workaround
            string xpath = $"//*/text()[contains(.,'{keyValuePair.Key}')]/following::span[contains(.,'{keyValuePair.Value}') and not(span)]";
            IWebElement check = driver.FindElement(By.XPath(xpath));
            return check;
        }

        private List<int> GetGeoInfo()
        {
            const string filename = "geoIndex.txt";
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
            if (_config.DriverTypes == DriverTypes.Chrome)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            }
            // IWebElement check = GetWebElement(new KeyValuePairString("所在地址", "点击获取")); 
            string selector = "body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(23) > div > input[type=text]";
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
                selector = "body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(24) > div > div > select.hcqbtn.hcqbtn-danger";
                IWebElement provinceRoot = driver.FindElement(By.CssSelector(selector));
                provinceRoot.Click();
                // select province
                selector = $"body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(24) > div > div > select.hcqbtn.hcqbtn-danger > option:nth-child({locationIndexes[0]})";
                IWebElement province = driver.FindElement(By.CssSelector(selector));
                province.Click();
                // select options list
                selector = "body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(24) > div > div > select.hcqbtn.hcqbtn-warning";
                IWebElement cityRoot = driver.FindElement(By.CssSelector(selector));
                cityRoot.Click();
                // select city
                selector = $"body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(24) > div > div > select.hcqbtn.hcqbtn-warning > option:nth-child({locationIndexes[1]})";
                IWebElement city = driver.FindElement(By.CssSelector(selector));
                city.Click();
                // select options list
                selector = "body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(24) > div > div > select.hcqbtn.hcqbtn-primary";
                IWebElement localRoot = driver.FindElement(By.CssSelector(selector));
                localRoot.Click();
                // select local
                selector = $"body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.form > ul > li:nth-child(24) > div > div > select.hcqbtn.hcqbtn-primary > option:nth-child({locationIndexes[2]})";
                IWebElement local = driver.FindElement(By.CssSelector(selector));
                local.Click();
            }
            catch (NoSuchElementException ex)
            {
                _logger.LogDebug(ex.Message);
                _logger.LogInformation("[Info] Geometry seems to be succeeded");
            }
            finally
            {
                if (_config.DriverTypes == DriverTypes.Chrome)
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_config.ElementDiscoveryTimeoutInSeconds);
                }
            }
        }

        private void Submit(IWebDriver driver)
        {
            // submit
            string selector = "body > div.item-buydate.form-detail2 > div:nth-child(1) > div > section > div.list-box > div > a";
            IWebElement submit = driver.FindElement(By.CssSelector(selector));
            submit.Click();
            try
            {
                // confirm
                selector = "#wapcf > div > div.wapcf-btn-box > div.wapcf-btn.wapcf-btn-ok";
                IWebElement confirm = driver.FindElement(By.CssSelector(selector));
                confirm.Click();
            }
            catch (NoSuchElementException ex)
            {
                _logger.LogDebug(ex.Message);
                const string warningMessage = "It seems that you have submitted the health report today.";
                _logger.LogWarning(warningMessage);
            }
        }
    }
}

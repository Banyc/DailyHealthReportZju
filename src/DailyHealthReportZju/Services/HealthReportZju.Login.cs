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

namespace DailyHealthReportZju.Services
{
    // only works on <https://healthreport.zju.edu.cn/ncov/wap/default/index>
    public partial class HealthReportZju
    {
        private readonly string _usernameSelector = "#username";
        private readonly string _passwordSelector = "#password";
        private readonly string _rememberMeSelector = "#fm1 > div.login-info.row > div.col-lg-3.col-md-3.col-sm-3.col-xs-4.remember-me > label";
        private readonly string _submitSelector = "#dl";

        private void OpenIdLogin(IWebDriver driver)
        {
            if (!driver.Url.StartsWith("https://zjuam.zju.edu.cn"))
            {
                const string errorMessage = "This page is not the verified authentication provider, thus is not trusted. The process ends. Do not enter your password in this page!";
                _logger.LogError(errorMessage);
                throw new Exception("Invalid authentication provider. " + errorMessage);
            }
            if (string.IsNullOrEmpty(_usernameSelector) || string.IsNullOrEmpty(_passwordSelector))
            {
                const string errorMessage = "You didn't set up username or password in configuration file \"appsettings.json\". Please login manually and restart the program.";
                _logger.LogError(errorMessage);
                throw new Exception("Login required. " + errorMessage);
            }
            var usernameControl = driver.FindElement(By.CssSelector(_usernameSelector));
            usernameControl.SendKeys(_config.Username);
            var passwordControl = driver.FindElement(By.CssSelector(_passwordSelector));
            passwordControl.SendKeys(_config.Password);
            var rememberMeControl = driver.FindElement(By.CssSelector(_rememberMeSelector));
            rememberMeControl.Click();
            var submitControl = driver.FindElement(By.CssSelector(_submitSelector));
            submitControl.Click();
        }
    }
}

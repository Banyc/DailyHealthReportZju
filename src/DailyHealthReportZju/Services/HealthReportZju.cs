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
using DailyHealthReportZju.Models;
using Microsoft.Extensions.Logging;
using DailyHealthReportZju.Helpers;
using Microsoft.Extensions.Options;

namespace DailyHealthReportZju.Services
{
    // only works on <https://healthreport.zju.edu.cn/ncov/wap/default/index>
    public partial class HealthReportZju
    {
        private readonly ChangeDetector<string> _changeDetector = new Helpers.ChangeDetector<string>();
        private readonly ILogger<HealthReportZju> _logger;
        private readonly HealthReportZjuSettings _config;

        public HealthReportZju(ILogger<HealthReportZju> logger,
            IOptions<HealthReportZjuSettings> config)
        {
            _logger = logger;
            _config = config.Value;
        }

        public void Start()
        {
            _logger.LogWarning("[Disclaimer] We (as the creator of this tool) has NO responsibility for any damages you suffer as a result of using our products or services");

            IWebDriver driver;
            try
            {
                driver = GetDriver(_config.Url);
                // driver.Manage().Window.Minimize();  // This will disable the whole auto process
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                const string errorMessage =
                @"Please close the previous chrome popup then restart this program (if possible).
                    If problem persists:
                        1. check if chrome is about to update;
                        2. update it.
                    If problem still persists, download chromedriver of corresponding version (link below) and replace the old one.
                        <https://chromedriver.chromium.org/downloads>
                    If problem still persists, reboot the computer.
                ";
                _logger.LogError(errorMessage);
                Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                return;
            }

            List<KeyValuePairString> keyWords = _config.KeyWords;

            // check if loginned
            try
            {
                // click all shits
                CheckAllOptions(driver, keyWords);
            }
            catch (NoSuchElementException)
            {
                // login
                OpenIdLogin(driver);
                // click all shits
                CheckAllOptions(driver, keyWords);
            }

            // select gep position
            SetGeo(driver);

            if (_config.IsFullAutoMode)
            {
                // submit
                Submit(driver);

                // process about to exit
                _logger.LogInformation("[Info] Done.");
                driver.Quit();
            }
        }
    }
}

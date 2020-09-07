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
            catch (Exception)
            {
                _logger.LogError("Please close the previous chrome popup then restart this program.");
                _logger.LogError("If problem persists, download chromedriver of corresponding version (link below) and replace the old one.");
                _logger.LogError("<https://chromedriver.chromium.org/downloads>");
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

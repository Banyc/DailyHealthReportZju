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
            // SemaphoreSlim videoStableSignal = new SemaphoreSlim(0, 1);

            // Console.WriteLine("[Info] This program could only handle one video once.");
            // Console.WriteLine("[Info] It won't automatically go to the next video.");
            // Console.WriteLine("[Info] It won't refresh the website when video is not playing either.");
            Console.WriteLine("[Info] [Disclaimer] We (as the creator of this tool) has NO responsibility for any damages you suffer as a result of using our products or services");

            IWebDriver driver;
            try
            {
                driver = GetDriver(_config.Url);
                // driver.Manage().Window.Minimize();  // This will disable the whole auto process
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR] Please close the previous chrome popup then restart this program.");
                Console.WriteLine("[ERROR] If problem persists, download chromedriver of corresponding version (link below) and replace the old one.");
                Console.WriteLine("[ERROR] <https://chromedriver.chromium.org/downloads>");
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
                Console.WriteLine("[Info] End of the the function `Start`");
                driver.Quit();
            }
        }
    }
}
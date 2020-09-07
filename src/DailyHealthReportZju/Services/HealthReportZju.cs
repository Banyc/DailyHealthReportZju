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
        private readonly Helpers.ChangeDetector<string> _changeDetector = new Helpers.ChangeDetector<string>();

        public HealthReportZju()
        {

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

            if (GlobalSettings.IsFullAutoMode)
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

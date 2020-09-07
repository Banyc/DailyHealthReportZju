using System.Linq;
using System.Collections.Generic;

namespace DailyHealthReportZju
{
    public enum DriverTypes
    {
        Chrome,
        // not tested
        Firefox,
    }

    public class KeyValuePairString
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class HealthReportZjuSettings
    {
        public bool IsFullAutoMode { get; set; }
        // In headless mode, no custom user profile is allowed
        // Headless mode is still buggy due to the nature of chromium
        public bool IsHeadless { get; set; }
        // Do NOT try Firefox that is not currently working.
        public DriverTypes DriverTypes { get; set; }
        public string FirefoxBrowserPath { get; set; }
        public string Url { get; set; }
        public int InitiationTimeoutInSeconds { get; set; }
        public int ElementDiscoveryTimeoutInSeconds { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<KeyValuePairString> KeyWords { get; set; }
    }
}

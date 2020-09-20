namespace DailyHealthReportZju.Models
{
    public class AppSettings
    {
        /// <summary>
        /// Sets header title in console window
        /// </summary>
        public string ConsoleTitle { get; set; }
        public bool IsHosted { get; set; }
        public int TriggeredHourUTC { get; set; }
    }
}

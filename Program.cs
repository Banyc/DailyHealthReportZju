using System;

namespace DailyHealthReportZju
{
    class Program
    {
        static void Main(string[] args)
        {
            HealthReportZju crawler = new HealthReportZju();
            crawler.Start();
        }
    }
}

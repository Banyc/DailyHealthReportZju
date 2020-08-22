using System;
using DailyHealthReportZju.Services;

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

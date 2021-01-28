using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DailyHealthReportZju.Models;
using DailyHealthReportZju.Services;

namespace DailyHealthReportZju
{
    public class App
    {
        private readonly HealthReportZju _healthReportService;
        private readonly ILogger<App> _logger;
        private readonly AppSettings _config;

        public App(HealthReportZju healthReportService,
            IOptions<AppSettings> config,
            ILogger<App> logger)
        {
            _healthReportService = healthReportService;
            _logger = logger;
            _config = config.Value;
        }

        public void Run()
        {
            _logger.LogInformation($"This is a console application for {_config.ConsoleTitle}");
            _healthReportService.Start();
            if (!_config.IsHosted)
            {
                return;
            }
            int hourToTask = (_config.TriggeredHourUTC - DateTime.UtcNow.Hour + 24) % 24;
            _logger.LogInformation($"Service will start after {hourToTask} hours");
            Task.Delay(TimeSpan.FromHours(hourToTask)).Wait();
            while (true)
            {
                _healthReportService.Start();
                Task.Delay(TimeSpan.FromDays(1)).Wait();
            }
        }
    }
}

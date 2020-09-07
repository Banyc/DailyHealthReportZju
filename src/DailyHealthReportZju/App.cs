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
        }
    }
}

using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DailyHealthReportZju.Models;
using DailyHealthReportZju.Services;

namespace DailyHealthReportZju
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // run app
            serviceProvider.GetService<App>().Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add logging
            serviceCollection.AddLogging(options =>
            {
                options.AddConsole();
                options.AddDebug();
            });

            // build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.secret.json", false)  // this config will overwrite the previous one if conflicting
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));
            serviceCollection.Configure<HealthReportZjuSettings>(configuration.GetSection("HealthReportZjuSettings"));
            ConfigureConsole(configuration);

            // add services
            serviceCollection.AddTransient<HealthReportZju>();

            // add app
            serviceCollection.AddTransient<App>();
        }

        private static void ConfigureConsole(IConfigurationRoot configuration)
        {
            System.Console.Title = configuration.GetSection("Configuration:ConsoleTitle").Value;
        }
    }
}

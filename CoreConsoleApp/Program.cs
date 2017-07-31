using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Logging;


namespace NZ01
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IConfigurationRoot _configuration;
        private static ILogger _logger;

        public static readonly string _fnSuffix = "() - ";

        static Program()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();

            // Configuration should be loaded before services are set up.
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // Logging
            var loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddLog4Net(); // Add Log4Net to the Factory
            loggerFactory.AddConsole(_configuration.GetSection("Logging"));

            _logger = loggerFactory.CreateLogger<Program>();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging(); // Adds the LoggerFactory

            services.Configure<ExampleOptions>(_configuration);

            // Create service objects with Lifetimes here...
            //services.AddSingleton<IEmailService, EmailService>();
            //services.AddSingleton<Work>();
        }



        static void Main(string[] args)
        {
            string prefix = nameof(Main) + _fnSuffix;

            _logger.LogDebug(prefix + "Entering");


            // Example: Find your service you created in ConfigureServices and make it do something
            //var work = _serviceProvider.GetService<Work>();
            //work.DoSomething();


            Console.WriteLine(prefix + "Press return to exit...");
            Console.ReadLine();

            _logger.LogDebug(prefix + "Exiting");

            Log4NetAsyncLog.Stop(); // Gracefully stop logs
        }

    } // end of class Program

} // end of namespace NZ01
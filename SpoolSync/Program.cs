using Tomlet;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

namespace SpoolSync
{
    public class Program
    {
        public static Settings AppSettings = new Settings();

        public static void Main(string[] args)
        {
            Console.WriteLine("Building SpoolSync.");
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddWindowsService(options =>
            {
                options.ServiceName = "SpoolSync Worker Service";
            });
            builder.Services.AddHostedService<Worker>();
            if (OperatingSystem.IsWindows())
            {
                LoggerProviderOptions.RegisterProviderOptions<
                    EventLogSettings, EventLogLoggerProvider>(builder.Services);
            }
            builder.Services.AddHostedService<Worker>();
            Console.WriteLine("Starting SpoolSync Worker Service...");
            Console.WriteLine("Loading configuration...");
            var configPath = Path.Combine(AppContext.BaseDirectory, "settings.toml");
            Console.WriteLine("Configuration path: " + configPath);
            
            if (File.Exists(configPath))
                AppSettings = TomletMain.To<Settings>(File.ReadAllText(configPath));
            else
                Console.WriteLine("Configuration file settings.toml not found. Using default settings.");
            Console.WriteLine($"SpoolMan API: {AppSettings.SpoolmanApi}.");
            Console.WriteLine($"Orca Path: {AppSettings.OrcaPath}");

            var host = builder.Build();
            host.Run();
        }
    }
}
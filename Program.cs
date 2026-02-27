using System;
using LogPad.Services;
using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;
using Serilog;

namespace LogPad
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("Logpad.log", 
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            try
            {
                Log.Information("LogPad application starting");

                var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

                appBuilder.Services
                    .AddScoped<LogFileService>()
                    .AddScoped<LogLineService>()
                    .AddScoped<LogStorageService>()
                    .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

                // register root component and selector
                appBuilder.RootComponents.Add<App>("app");

                var app = appBuilder.Build();

                // customize window
                app.MainWindow
                    .SetIconFile("favicon.ico")
                    .SetTitle("LogPad")
                    .SetUseOsDefaultSize(false)
                    .SetMaximized(true);

                AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
                {
                    Log.Fatal(error.ExceptionObject as Exception, "Unhandled exception occurred");
                    app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
                };

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
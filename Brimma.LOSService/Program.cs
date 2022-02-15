using Brimma.LOSService.Common;
using Brimma.LOSService.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using System;

namespace Brimma.LOSService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                    var builtConfig = config.Build();
                    logger.Error("EDS Service - Program File - Azure Key Vault Information Log - URL: {0}", builtConfig["AzureKeyVault:AccountEndpoint"]);
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    try
                    {
                        if (environment == "Development")
                        {
                            config.AddAzureKeyVault(builtConfig["AzureKeyVault:AccountEndpoint"], builtConfig["AzureKeyVault:ClientId"],
                            builtConfig["AzureKeyVault:ClientSecret"]);
                        }
                        else
                        {
                            config.AddAzureKeyVault(builtConfig["AzureKeyVault:AccountEndpoint"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        //NLog: catch setup errors
                        logger.Error(ex, "Error Occurred at EDS Service - Exception Thrown in Program.CS file while adding Azure KeyVault");
                        throw;
                    }

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Limits.MaxRequestBodySize = null;
                        options.Limits.MaxRequestBufferSize = null;
                    })
                    .UseStartup<Startup>();
                });
    }

}

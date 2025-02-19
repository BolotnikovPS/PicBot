using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PicBot.Domain.Enums;
using PicBot.Infrastructure.Dependencies;
using Prometheus;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using TBotPlatform.Extension;

namespace PicBot.Bootstrap;

public static class HostExtensions
{
    public static IHostBuilder CreateHost(EBotType botType)
    {
        return Host
              .CreateDefaultBuilder()
              .UseDefaultServiceProvider(
                   (_, options) => { options.ValidateOnBuild = false; })
              .UseConsoleLifetime()
              .ConfigureAppConfiguration(
                   config => { config.AddConfiguration(ResolveConsulConfiguration(botType)); })
              .ConfigureServices(
                   (context, services) =>
                   {
                       services
                          .Configure<KestrelServerOptions>(context.Configuration.GetSection("Kestrel"))
                          .AddInfrastructure(botType)
                          .AddHealthChecks()
                          .ForwardToPrometheus();
                   })
              .UseSerilog(
                   (context, configuration) =>
                   {
                       configuration.ReadFrom.Configuration(context.Configuration);
                       var formatter = new JsonFormatter();

                       configuration.WriteTo.Logger(
                           l => l
                               .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                               .WriteTo.File(
                                    formatter,
                                    "Logs/information-.txt",
                                    rollingInterval: RollingInterval.Day)
                           );

                       configuration.WriteTo.Logger(
                           l => l
                               .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                               .WriteTo.File(
                                    formatter,
                                    "Logs/error-.txt",
                                    rollingInterval: RollingInterval.Day)
                           );

                       configuration.WriteTo.Logger(
                           l => l
                               .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                               .WriteTo.File(
                                    formatter,
                                    "Logs/debug-.txt",
                                    rollingInterval: RollingInterval.Day)
                           );

                       configuration.WriteTo.Logger(
                           l => l
                               .Filter.ByIncludingOnly(e => e.Level.NotIn(LogEventLevel.Information, LogEventLevel.Error, LogEventLevel.Debug))
                               .WriteTo.File(
                                    formatter,
                                    "Logs/other-.txt",
                                    rollingInterval: RollingInterval.Day)
                           );
                   });
    }

    private static IConfiguration ResolveConsulConfiguration(EBotType botType)
    {
        var configuration = new ConfigurationBuilder()
                           .SetBasePath(AppContext.BaseDirectory)
                           .AddJsonFile("appsettings.json")
                           .Build();

        return new ConfigurationBuilder()
              .AddConsulConfig(configuration, botType)
              .AddConfiguration(configuration)
              .Build();
    }
}
using PicBot.Infrastructure.Dependencies;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Serilog;
using PicBot.Domain.Enums;

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
              .UseSerilog();
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
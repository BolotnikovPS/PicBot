using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using PicBot.Application.Abstractions;
using PicBot.Application.Bots;
using PicBot.Application.CQ.Behaviour;
using PicBot.Application.Templates;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Abstractions.Publishers.EventDomain;
using PicBot.Domain.Bots.Config;
using PicBot.Domain.Enums;
using System.Reflection;
using TBotPlatform.Common.Dependencies;

namespace PicBot.Application.Dependencies;

public static partial class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, EBotType botType)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        services
           .AddSingleton<IBotType, BotType>(
                serviceProvider =>
                {
                    var configService = serviceProvider.GetRequiredService<IConfigService>();

                    var botSettings = configService.GetValueOrNull<BotSettings>(EConfigKey.BotSettings);
                    return new(new(botType, botSettings));
                })
           .AddMediatR(
                cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(executingAssembly);
                    cfg.NotificationPublisher = new TaskWhenAllPublisher();
                })
           .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
           .AddScoped<IEventDomainPublisher, DomainEventPublisher>()
           .AddHelpers()
           .AddStates(executingAssembly, botType.ToString())
           .AddFactories(executingAssembly)
           .AddReceivingHandler<StartReceivingHandler>();

        return services;
    }
}
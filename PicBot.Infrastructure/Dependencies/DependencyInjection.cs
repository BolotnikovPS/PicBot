using PicBot.Application.Dependencies;
using Microsoft.Extensions.DependencyInjection;
using PicBot.Application.Abstractions;
using PicBot.Domain.Bots.Config;
using PicBot.Domain.Enums;
using TBotPlatform.Common.Dependencies;
using TBotPlatform.Contracts.Bots.Config;

namespace PicBot.Infrastructure.Dependencies;

public static partial class DependencyInjection
{
    private static readonly string[] Tags = ["readiness",];

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, EBotType botType)
    {
        services.AddConfigService();

        var configService = services.BuildServiceProvider().GetRequiredService<IConfigService>();

        var redisConnectionString = GetValueOrException(configService, EConfigKey.Redis);
        var telegram = GetValueOrException<TelegramSettings>(configService, EConfigKey.Telegram);


        services
           .AddTelegramContextHostedService(telegram)
           .AddCache(redisConnectionString, prefix: botType.ToString(), tags: Tags)
           .AddBotTypeInfrastructure(configService, Tags)
           .AddHttpClient<IImageService, ImageService>(
                nameof(ImageService),
                client =>
                {
                    client.BaseAddress = new("https://yandex.ru/");
                });

        services
           .AddApplication(botType)
           .AddQuartzScheduler(botType);

        return services;
    }
}
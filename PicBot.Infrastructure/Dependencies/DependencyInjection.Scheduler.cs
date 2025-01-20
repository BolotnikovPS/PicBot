using Microsoft.Extensions.DependencyInjection;
using PicBot.Application.Abstractions;
using PicBot.Domain.Abstractions.Helpers;
using PicBot.Domain.Enums;
using PicBot.Infrastructure.Scheduler;
using Quartz;

namespace PicBot.Infrastructure.Dependencies;

public static partial class DependencyInjection
{
    internal static IServiceCollection AddQuartzScheduler(this IServiceCollection services, EBotType botType)
    {
        var dateTimeHelper = services.BuildServiceProvider().GetRequiredService<IDateTimeHelper>();

        var dateTimeNow = dateTimeHelper.GetUtcDateTimeNow();

        var startAtMinutely = CreateStartDateTimeForMinutely(dateTimeNow);
        var startAtHour = CreateStartDateTimeForHour(dateTimeNow);
        var startAtNow = dateTimeNow.AddSeconds(20);

        CronScheduleBuilder.DailyAtHourAndMinute(0, 0);

        services
           .AddQuartz(
                options =>
                {
                    options.SchedulerId = botType.ToString();
                })
           .AddQuartzHostedService(options => options.WaitForJobsToComplete = true)
           .AddScoped<ISchedulerJobFactory, SchedulerJobFactory>();

        return services;
    }

    private static DateTime CreateStartDateTimeForMinutely(DateTime dateTimeNow)
    {
        var result = dateTimeNow.AddMinutes(3).AddSeconds(-dateTimeNow.Second);

        if (result.Minute % 5.0 == 0)
        {
            return result;
        }

        var newMinute = Convert.ToInt32(Math.Ceiling(result.Minute / 5.0) * 5);

        return newMinute >= 60
            ? result.AddHours(1).AddMinutes(-result.Minute)
            : result.AddMinutes(newMinute - result.Minute);
    }

    private static DateTime CreateStartDateTimeForHour(DateTime dateTimeNow)
        => dateTimeNow.AddHours(1).AddMinutes(-dateTimeNow.Minute).AddSeconds(-dateTimeNow.Second);

    private static void AddMinutely<T>(
        this IServiceCollectionQuartzConfigurator serviceCollectionQuartzConfigurator,
        int minutes,
        string description,
        DateTime startAt
        )
        where T : IJob
    {
        var type = typeof(T).GenericTypeArguments.FirstOrDefault()?.Name;

        serviceCollectionQuartzConfigurator.ScheduleJob<T>(
            trigger =>
            {
                trigger
                   .WithIdentity(type)
                   .WithDescription(description)
                   .StartAt(startAt)
                   .WithSimpleSchedule(x => x.WithIntervalInMinutes(minutes).RepeatForever());
            },
            jobConfigurator =>
            {
                jobConfigurator
                   .WithIdentity(type)
                   .WithDescription(description);
            });
    }

    private static void AddHours<T>(
        this IServiceCollectionQuartzConfigurator serviceCollectionQuartzConfigurator,
        int hours,
        string description,
        DateTime startAt
        )
        where T : IJob
    {
        var type = typeof(T).GenericTypeArguments.FirstOrDefault()?.Name;

        serviceCollectionQuartzConfigurator.ScheduleJob<T>(
            trigger =>
            {
                trigger
                   .WithIdentity(type)
                   .WithDescription(description)
                   .StartAt(startAt)
                   .WithSimpleSchedule(x => x.WithIntervalInHours(hours).RepeatForever());
            },
            jobConfigurator =>
            {
                jobConfigurator
                   .WithIdentity(type)
                   .WithDescription(description);
            });
    }

    private static void AddDaily<T>(
        this IServiceCollectionQuartzConfigurator serviceCollectionQuartzConfigurator,
        int day,
        string description,
        DateTime dateTimeNow,
        int hour,
        int minute
        )
        where T : IJob
    {
        var type = typeof(T).GenericTypeArguments.FirstOrDefault()?.Name;

        serviceCollectionQuartzConfigurator.ScheduleJob<T>(
            trigger =>
            {
                trigger
                   .WithIdentity(type)
                   .WithDescription(description)
                   .StartAt(new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, hour, minute, 0))
                   .WithSimpleSchedule(x => x.WithIntervalInHours(day * 24).RepeatForever());
            },
            jobConfigurator =>
            {
                jobConfigurator
                   .WithIdentity(type)
                   .WithDescription(description);
            });
    }
}
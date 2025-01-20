using PicBot.Application.Abstractions;
using PicBot.Domain.Bots;
using Quartz;
using Quartz.Impl.Matchers;
using TBotPlatform.Extension;

namespace PicBot.Infrastructure.Scheduler;

internal class SchedulerJobFactory(ISchedulerFactory schedulerFactory) : ISchedulerJobFactory
{
    public async Task<List<BotJobData>> GetJobListAsync(CancellationToken cancellationToken)
    {
        var result = await GetJobDetailListAsync(cancellationToken);
        return result.ConvertAll(
            z => new BotJobData
            {
                Description = z.Description,
                Name = z.Key.Name,
            });
    }

    public async Task StartJobAsync(BotJobData botJobData, CancellationToken cancellationToken)
    {
        var result = await GetJobDetailListAsync(cancellationToken);
        var jobKey = result.Find(z => z.Key.Name == botJobData.Name).Key;

        if (jobKey.IsNull())
        {
            return;
        }

        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
        await scheduler.TriggerJob(jobKey, cancellationToken);
    }


    private async Task<List<IJobDetail>> GetJobDetailListAsync(CancellationToken cancellationToken)
    {
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
        var jobsKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellationToken);

        var result = new List<IJobDetail>();

        foreach (var key in jobsKeys)
        {
            var detail = await scheduler.GetJobDetail(key, cancellationToken);
            result.Add(detail);
        }

        return result;
    }
}
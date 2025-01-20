using PicBot.Domain.Bots;

namespace PicBot.Application.Abstractions;

public interface ISchedulerJobFactory
{
    Task<List<BotJobData>> GetJobListAsync(CancellationToken cancellationToken);
    Task StartJobAsync(BotJobData botJobData, CancellationToken cancellationToken);
}
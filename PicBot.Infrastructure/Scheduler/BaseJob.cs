using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PicBot.Application.Abstractions.DBContext;
using PicBot.Domain.Abstractions.Publishers.EventDomain;
using Quartz;
using TBotPlatform.Contracts.Abstractions.Factories;
using TBotPlatform.Extension;

namespace PicBot.Infrastructure.Scheduler;

internal sealed class BaseJob<T>(
    ILogger<BaseJob<T>> logger,
    IBotPlatformDbContext botPlatformDbContext,
    IStateContextFactory stateContextFactory,
    IEventDomainPublisher eventDomainPublisher
    ) : IJob
    where T : IEventDomainMessage
{
    public async Task Execute(IJobExecutionContext context)
    {
        var sbText = new StringBuilder($"✅ Выполнилась задача {context!.JobDetail.Description.ToBoldString()}");

        var timer = Stopwatch.StartNew();

        var executeWithError = false;
        Exception exception = null;
        try
        {
            var message = Activator.CreateInstance<T>();
            await eventDomainPublisher.PublishAsync(message, context.CancellationToken);
        }
        catch (Exception ex)
        {
            exception = ex;
            executeWithError = true;

            sbText.AppendLine("😬 При выполнении задачи возникло исключение");
        }
        finally
        {
            timer.Stop();

            sbText
               .AppendLine()
               .AppendLine($"Время выполнения: {timer.Elapsed}")
               .AppendLine()
               .AppendLine($"#{context.JobDetail.Key.Name}");

            if (executeWithError)
            {
                logger.LogError(exception, "{sbText}", sbText.ToString());
            }
            else
            {
                logger.LogInformation("{sbText}", sbText.ToString());
            }

            await SendAdminAsync(sbText, context.CancellationToken);
        }
    }

    private async Task SendAdminAsync(StringBuilder sbText, CancellationToken cancellationToken)
    {
        var users = await botPlatformDbContext.Users.ToListAsync(cancellationToken);

        foreach (var admin in users.Where(z => z.IsAdmin()))
        {
            try
            {
                await using var stateContext = stateContextFactory.GetStateContext(admin);
                await stateContext.SendTextMessage(sbText.ToString(), cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при уведомлении админа: {admin}", admin.ToJson());
            }
        }
    }
}
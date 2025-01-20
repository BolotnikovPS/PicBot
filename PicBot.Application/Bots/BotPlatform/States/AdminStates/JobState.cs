using PicBot.Application.Attributes;
using PicBot.Domain.Bots;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Markups;
using TBotPlatform.Contracts.Bots.Markups.InlineMarkups;
using TBotPlatform.Extension;
using PicBot.Domain.Abstractions.Helpers;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Enums;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Application.Bots.BotPlatform.States.MessageStates;
using PicBot.Application.Abstractions;
using PicBot.Application.Extensions;

namespace PicBot.Application.Bots.BotPlatform.States.AdminStates;

[MyStateInlineActivator(ButtonsTypes = [EButtonsType.ListJobs,])]
internal class JobState(ISchedulerJobFactory schedulerFactory, IDateTimeHelper dateTimeHelper) : IMyState
{
    private const string NoJobText = "Нет джобов.";

    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var jobs = await schedulerFactory.GetJobListAsync(cancellationToken);

        if (jobs.IsNull())
        {
            await context.SendTextMessageAsync(NoJobText, cancellationToken);

            return;
        }

        if (context.MarkupNextState.IsNull())
        {
            var inlineButtons = new InlineMarkupList();

            foreach (var job in jobs)
            {
                inlineButtons.Add(new InlineMarkupState(job.Description, nameof(JobState), job.Name));
            }

            inlineButtons.Add(new MyInlineMarkupState(EInlineButtonsType.ToClose, nameof(MessageCloseState)));

            await context.SendOrUpdateTextMessageAsync($"Список джобов на {dateTimeHelper.GetLocalDateTimeNow().ToRussianWithHours()}", inlineButtons, null, cancellationToken);

            return;
        }

        if (context.MarkupNextState.Data.CheckAny())
        {
            var job = jobs.Find(z => z.Name == context.MarkupNextState.Data);

            if (job.IsNull())
            {
                await context.SendOrUpdateTextMessageAsync($"🛑 Задача {context.MarkupNextState.Data} не найдена.", cancellationToken);

                return;
            }

            await schedulerFactory.StartJobAsync(job, cancellationToken);

            await context.SendOrUpdateTextMessageAsync($"💪 Задача {job.Name} запущена.", cancellationToken);
        }
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
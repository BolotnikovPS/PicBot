using System.Text;
using PicBot.Application.Attributes;
using PicBot.Application.CQ.DbContext.BotPlatformContext.Queries;
using PicBot.Application.Extensions;
using PicBot.Domain.Bots;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Markups;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Enums;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Application.Bots.BotPlatform.States.MessageStates;

namespace PicBot.Application.Bots.BotPlatform.States.AdminStates.UserStates.Statistics;

[MyStateInlineActivator(ButtonsTypes = [EButtonsType.GetUsersStatistic,])]
internal class ShortUsersStatisticState(IMediator mediator) : IMyState
{
    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new UsersQuery(), cancellationToken);

        var sbText = new StringBuilder($"Всего пользователей: {users.Count}")
                    .AppendLine()
                    .AppendLine($"Заблокированных пользователей: {users.Count(z => z.IsLock())}")
                    .AppendLine()
                    .AppendLine($"Админов: {users.Count(z => z.IsAdmin())}");

        var inlineButtons = new InlineMarkupList
        {
            new MyInlineMarkupState(EInlineButtonsType.UsersDetailedStatistics, nameof(FullUsersStatisticState)),
            new MyInlineMarkupState(EInlineButtonsType.ToClose, nameof(MessageCloseState)),
        };

        await context.SendOrUpdateTextMessageAsync(sbText.ToString(), inlineButtons, cancellationToken);
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
using PicBot.Application.Attributes;
using PicBot.Application.CQ.DbContext.BotPlatformContext.Commands;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using PicBot.Domain.Contexts.BotPlatform.Enums;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;

namespace PicBot.Application.Bots.BotPlatform.States.AdminStates.UserStates.Lockers;

[MyStateInlineActivator]
internal class ToLockUserState(IMediator mediator) : IMyState
{
    private const string Text = "Пользователь заблокирован.";

    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await mediator.Send(
            new UpdateUserCommand(long.Parse(context.MarkupNextState.Data), EUserBlockType.Fraud),
            cancellationToken
            );

        await context.UpdateMarkupTextAndDropButtonAsync(Text, cancellationToken);
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
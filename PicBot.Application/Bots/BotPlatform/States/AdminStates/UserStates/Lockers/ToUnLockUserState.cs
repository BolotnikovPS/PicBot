using PicBot.Application.Attributes;
using PicBot.Application.CQ.DbContext.BotPlatformContext.Commands;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using PicBot.Domain.Contexts.BotPlatform.Enums;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;

namespace PicBot.Application.Bots.BotPlatform.States.AdminStates.UserStates.Lockers;

[MyStateInlineActivator]
internal class ToUnLockUserState(IMediator mediator) : IMyState
{
    private const string Text = "Пользователь разблокирован.";

    public async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateUserCommand(long.Parse(context.MarkupNextState.Data), EUserBlockType.None), cancellationToken);

        await context.UpdateMarkupTextAndDropButton(Text, cancellationToken);
    }

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
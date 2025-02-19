using MediatR;
using PicBot.Application.Attributes;
using PicBot.Application.CQ.DbContext.BotPlatformContext.Commands;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Contexts.BotPlatform.Enums;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace PicBot.Application.Bots.BotPlatform.States.AdminStates.UserStates.Lockers;

[MyStateInlineActivator]
internal class ToLockUserState(IMediator mediator) : IMyState
{
    private const string Text = "Пользователь заблокирован.";

    public async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await mediator.Send(
            new UpdateUserCommand(long.Parse(context.MarkupNextState.Data), EUserBlockType.Fraud),
            cancellationToken
            );

        await context.UpdateMarkupTextAndDropButton(Text, cancellationToken);
    }

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
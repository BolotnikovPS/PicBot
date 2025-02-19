using PicBot.Application.Attributes;
using PicBot.Application.Bots.BotPlatform.MenuButton;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace PicBot.Application.Bots.BotPlatform.States.AdminStates;

[MyStateActivator(typeof(AdminMenuButton), ButtonsTypes = [EButtonsType.Admin,])]
internal class AdminState : IMyState
{
    public Task Handle(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
using System.Text;
using PicBot.Application.Attributes;
using PicBot.Application.CQ.DbContext.BotPlatformContext.Queries;
using PicBot.Application.Extensions;
using PicBot.Domain.Bots;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Markups;
using PicBot.Domain.Contexts.BotPlatform.Enums;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Enums;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Application.Bots.BotPlatform.States.AdminStates.UserStates.Lockers;

namespace PicBot.Application.Bots.BotPlatform.States.AdminStates.UserStates;

[MyStateInlineActivator]
internal class UserInfoState(IMediator mediator) : IMyState
{
    public async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var userFromState = await mediator.Send(
            new UserQuery(null, null, long.Parse(context.MarkupNextState.Data)),
            cancellationToken
            );

        var messageUserInfo = new StringBuilder($"Пользователь {userFromState.UserName}: {userFromState.FirstName} {userFromState.LastName}")
                             .AppendLine($"Тип пользователя: {userFromState.Role.ToString()}")
                             .AppendLine($"Блокировка: {userFromState.IsLock().ToString()}")
                             .AppendLine($"Дата регистрации: {userFromState.RegisterDate.ToRussian()}")
                             .AppendLine($"Id пользователя: {userFromState.TgUserId}")
                             .AppendLine($"Id чата: {userFromState.ChatId}")
                             .AppendLine()
                             .ToString();

        var userId = userFromState.Id.ToString();
        var buttons = new InlineMarkupList();

        if (userFromState.BlockType != EUserBlockType.KickedByUser)
        {
            buttons.Add(
                new MyInlineMarkupState(
                    userFromState.IsLock() ? EInlineButtonsType.ToUnLock : EInlineButtonsType.ToLock,
                    userFromState.IsLock() ? nameof(ToUnLockUserState) : nameof(ToLockUserState),
                    userId
                    )
                );
        }

        if (userFromState.BlockType == EUserBlockType.None)
        {
            buttons.Add(new MyInlineMarkupState(EInlineButtonsType.RefreshUserMenu, nameof(RefreshUserMenuState), userId));
        }

        buttons.Add(
            new MyInlineMarkupState(
                EInlineButtonsType.ToBack,
                userFromState.IsLock() ? nameof(NotActiveUsersState) : nameof(ActiveUsersState)
                )
            );

        await context.SendOrUpdateTextMessage(messageUserInfo, buttons, cancellationToken);
    }

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
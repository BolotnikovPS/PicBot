﻿using PicBot.Application.Attributes;
using PicBot.Application.CQ.DbContext.BotPlatformContext.Queries;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Pagination;
using TBotPlatform.Extension;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Enums;
using PicBot.Application.Extensions;

namespace PicBot.Application.Bots.BotPlatform.States.AdminStates.UserStates;

[MyStateInlineActivator(ButtonsTypes = [EButtonsType.GetAllUsers,])]
internal class AllUsersState(IMediator mediator) : IMyState
{
    public async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new UsersQuery(), cancellationToken);
        users = users.Where(z => z.Id != user.Id).ToList();

        if (users.IsNull())
        {
            await context.SendTextMessage("Список всех пользователей пустой", cancellationToken);

            return;
        }

        if (context.MarkupNextState.TryParsePagination(out var result))
        {
            await context.SendOrUpdateTextMessage("Список всех пользователей в боте.", users.CreateAllUserButtons(result), cancellationToken);

            return;
        }

        await context.SendOrUpdateTextMessage("Список всех пользователей в боте.", users.CreateAllUserButtons(1), cancellationToken);
    }

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
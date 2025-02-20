﻿using PicBot.Application.Attributes;
using PicBot.Application.CQ.DbContext.BotPlatformContext.Queries;
using PicBot.Application.Extensions;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Pagination;
using TBotPlatform.Extension;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;

namespace PicBot.Application.Bots.BotPlatform.States.AdminStates.UserStates;

[MyStateInlineActivator]
internal class NotActiveUsersState(IMediator mediator) : IMyState
{
    public async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new UsersQuery(), cancellationToken);
        users = users
               .Where(
                    z => z.Id != user.Id
                         && z.IsLock()
                    )
               .ToList();

        if (users.IsNull())
        {
            await context.SendTextMessage("Список заблокированных пользователей пустой", cancellationToken);

            return;
        }

        if (context.MarkupNextState.TryParsePagination(out var result))
        {
            await context.SendOrUpdateTextMessage("Список заблокированных пользователей в боте.", users.CreateNotActiveUserButtons(result), cancellationToken);

            return;
        }

        await context.SendOrUpdateTextMessage("Список заблокированных пользователей в боте.", users.CreateNotActiveUserButtons(1), cancellationToken);
    }

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
using PicBot.Application.Attributes;
using PicBot.Application.Contracts.Messages.DomainMessage;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Abstractions.Publishers.EventDomain;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Cache;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Extension;

namespace PicBot.Application.Bots.BotPlatform.States.AdminStates.UserStates;

[MyStateInlineActivator]
internal class RefreshUserMenuState(IEventDomainPublisher domainPublisher, ICacheService cacheService) : MyBaseState(cacheService), IMyState
{
    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        if (context.MarkupNextState.IsNull()
            || context.MarkupNextState.Data.IsNull()
           )
        {
            return;
        }

        await domainPublisher.PublishAsync(new RefreshMenuMessage(int.Parse(context.MarkupNextState.Data)), cancellationToken);

        await context.SendTextMessageAsync(GetDescription(ETextsType.MenuIsRefresh), cancellationToken);
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
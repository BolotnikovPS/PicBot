using PicBot.Application.Attributes;
using PicBot.Application.Contracts.Messages.DomainMessage;
using PicBot.Domain.Bots;
using TBotPlatform.Contracts.Abstractions.Cache;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Markups;
using TBotPlatform.Extension;
using PicBot.Domain.Abstractions.Publishers.EventDomain;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Enums;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Application.Bots.BotPlatform.States.MessageStates;

namespace PicBot.Application.Bots.BotPlatform.States.AdminStates;

[MyStateInlineActivator(ButtonsTypes = [EButtonsType.RefreshMenu,])]
internal class RefreshMenuState(IEventDomainPublisher domainPublisher, ICacheService cacheService) : MyBaseState(cacheService), IMyState
{
    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        if (context.MarkupNextState.IsNotNull())
        {
            await domainPublisher.PublishAsync(new RefreshMenuMessage(), cancellationToken);

            await context.SendOrUpdateTextMessageAsync(GetDescription(ETextsType.MenuIsRefresh), cancellationToken);

            return;
        }

        var buttons = new InlineMarkupList
        {
            new MyInlineMarkupState(EInlineButtonsType.Yes, nameof(RefreshMenuState)),
            new MyInlineMarkupState(EInlineButtonsType.ToClose, nameof(MessageCloseState)),
        };

        await context.SendOrUpdateTextMessageAsync(GetDescription(ETextsType.IsRefreshMenu), buttons, cancellationToken);
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
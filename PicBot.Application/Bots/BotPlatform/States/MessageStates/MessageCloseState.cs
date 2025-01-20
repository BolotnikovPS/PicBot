using PicBot.Application.Attributes;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.EnumCollection;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts;
using TBotPlatform.Contracts.Abstractions.Cache;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace PicBot.Application.Bots.BotPlatform.States.MessageStates;

[MyStateInlineActivator]
internal class MessageCloseState(ICacheService cacheService) : MyBaseState(cacheService), IMyState
{
    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await context.UpdateMarkupTextAndDropButtonAsync(InlineButtonsCollection.Instance.GetValueByKey(EInlineButtonsType.ToClose), cancellationToken);

        context.SetNeedUpdateMarkup();

        await RemoveValuesInCacheAsync(user.Id, cancellationToken);
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
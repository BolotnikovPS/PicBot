using PicBot.Domain.EnumCollection;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Cache;
using TBotPlatform.Contracts.Bots.States;

namespace PicBot.Application.Bots.BotPlatform;

internal abstract class MyBaseState(ICacheService cacheService)
    : BaseState(cacheService)
{
    protected static string GetDescription(ETextsType key)
        => TextCollection.Instance.GetValueByKey(key);
}
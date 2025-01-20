using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Bots;

namespace PicBot.Application.Bots;

internal class BotType(BotTypeData botTypeData) : IBotType
{
    public BotTypeData GetBotType() => botTypeData;
}
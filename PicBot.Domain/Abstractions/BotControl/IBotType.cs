using PicBot.Domain.Bots;

namespace PicBot.Domain.Abstractions.BotControl;

public interface IBotType
{
    BotTypeData GetBotType();
}
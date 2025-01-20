using PicBot.Domain.Bots.Config;
using PicBot.Domain.Enums;

namespace PicBot.Domain.Bots;

public sealed class BotTypeData(EBotType botType, BotSettings botSetting)
{
    public EBotType BotType { get; } = botType;
    public BotSettings BotSetting { get; } = botSetting;
}
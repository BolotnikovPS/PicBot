using TBotPlatform.Contracts.Bots.ChatUpdate.Enums;

namespace PicBot.Domain.Bots.Config;

public class BotSettings
{
    public bool WithRegistration { get; set; }

    public List<EChatType> ChatTypes { get; set; }
}
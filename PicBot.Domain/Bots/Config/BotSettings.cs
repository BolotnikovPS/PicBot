using Telegram.Bot.Types.Enums;

namespace PicBot.Domain.Bots.Config;

public class BotSettings
{
    public bool WithRegistration { get; set; }

    public List<ChatType> ChatTypes { get; set; }
}
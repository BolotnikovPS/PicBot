using PicBot.Domain.Contexts.BotPlatform.Enums;
using TBotPlatform.Contracts.Bots.Users;
using TBotPlatform.Extension;

namespace PicBot.Domain.Contexts.BotPlatform;

public class User : UserBase
{
    public int Id { get; set; }

    public EUserRoles Role { get; set; }

    public EUserBlockType? BlockType { get; set; }

    public DateTime? RegisterDate { get; set; }

    public override bool IsAdmin() => Role.In(EUserRoles.Admin);
}
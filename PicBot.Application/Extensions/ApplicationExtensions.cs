using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Contexts.BotPlatform.Enums;

namespace PicBot.Application.Extensions;

internal static class ApplicationExtensions
{
    public static bool IsLock(this User user) => user.BlockType != EUserBlockType.None;
}
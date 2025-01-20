using System.Collections.Frozen;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.EnumCollection;

namespace PicBot.Domain.EnumCollection;

public class CommandCollection : CollectionBase<ECommandsType>
{
    protected override FrozenDictionary<ECommandsType, string> DataCollection { get; } = new Dictionary<ECommandsType, string>
    {
        [ECommandsType.Start] = "/start",
        [ECommandsType.FindImage] = "findimage", //внутрення команда
        [ECommandsType.SaveImage] = "saveimage", //внутрення команда
        [ECommandsType.MyImage] = "/my",
    }.ToFrozenDictionary();

    public static CommandCollection Instance { get; } = new();
}
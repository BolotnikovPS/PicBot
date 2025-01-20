using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PicBot.Domain.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum EInlineButtonsType
{
    None = 0,
    ToClose,
    ToBack,
    GetActiveUsers,
    GetNotActiveUsers,
    ToLock,
    ToUnLock,
    Yes,
    UsersDetailedStatistics,
    RefreshUserMenu,
}
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PicBot.Domain.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum EButtonsType
{
    None = 0,
    ToBackMain,
    ToBack,
    Admin,
    ListJobs,
    GetUsersStatistic,
    GetAllUsers,
    RefreshMenu,
}
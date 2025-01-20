using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PicBot.Domain.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum ETextsType
{
    None = 0,
    IsRefreshMenu,
    MenuIsRefresh,
}
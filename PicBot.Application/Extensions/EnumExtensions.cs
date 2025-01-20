using PicBot.Domain.EnumCollection;
using PicBot.Domain.Enums;
using TBotPlatform.Extension;

namespace PicBot.Application.Extensions;

internal static class EnumExtensions
{
    /// <summary>
    /// Проверяет наличие input в value без регистровой зависимости
    /// </summary>
    /// <param name="input"></param>
    /// <param name="value"></param>
    /// <returns>bool</returns>
    internal static bool InButtonCollection(this string input, params EButtonsType[] value)
    {
        if (input.IsNull()
            || value.IsNull()
           )
        {
            return false;
        }

        return value
              .Select(
                   z => ButtonCollection
                       .Instance.GetValueByKey(z)
                       .ToUpper()
                   )
              .Contains(input.ToUpper());
    }

    internal static bool InTextCollection(this string input, params ETextsType[] value)
    {
        if (input.IsNull()
            || value.IsNull()
           )
        {
            return false;
        }

        return value
              .Select(
                   z => TextCollection
                       .Instance.GetValueByKey(z)
                       .ToUpper()
                   )
              .Contains(input.ToUpper());
    }

    internal static bool InCommandCollection(this string input, params ECommandsType[] value)
    {
        if (input.IsNull()
            || value.IsNull()
           )
        {
            return false;
        }

        return value
              .Select(
                   z => CommandCollection
                       .Instance.GetValueByKey(z)
                       .ToUpper()
                   )
              .Contains(input.ToUpper());
    }

    public static ETextsType ToTextType(this string value) => TextCollection.Instance.GetKeyByValue(value);
}
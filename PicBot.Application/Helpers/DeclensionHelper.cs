using PicBot.Domain.Abstractions.Helpers;
using PicBot.Domain.Enums;

namespace PicBot.Application.Helpers;

internal class DeclensionHelper : IDeclensionHelper
{
    string IDeclensionHelper.Decline(int value, EDeclensionType type)
    {
        return type switch
        {
            EDeclensionType.Day => NumDeclension(value, "день", "дней", "дня"),
            EDeclensionType.Hour => NumDeclension(value, "час", "часов", "часа"),
            EDeclensionType.Minute => NumDeclension(value, "минут", "минут", "минут"),
            _ => string.Empty,
        };
    }

    /// <summary>
    /// Возвращает слова в падеже, зависимом от заданного числа
    /// </summary>
    /// <param name="number">Число от которого зависит выбранное слово</param>
    /// <param name="nominative">Именительный падеж слова. Например "день"</param>
    /// <param name="plural">Множественное число слова. Например "дней"</param>
    /// <param name="genitive">Родительный падеж слова. Например "дня"</param>
    /// <returns></returns>
    private static string NumDeclension(int number, string nominative, string plural, string genitive = null)
    {
        if (string.IsNullOrEmpty(genitive))
        {
            return number == 1 ? nominative : plural;
        }

        var titles = new[] { nominative, genitive, plural, };
        var cases = new[] { 2, 0, 1, 1, 1, 2, };

        return titles[number % 100 > 4
                      && number % 100 < 20
                          ? 2
                          : cases[number % 10 < 5
                                      ? number % 10
                                      : 5]];
    }
}
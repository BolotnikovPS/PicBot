namespace PicBot.Application.Extensions;

internal static class DateTimeExtensions
{
    internal static string ToRussian(this DateTime type) => type.ToString("dd.MM.yyyy");

    internal static string ToRussian(this DateTime? type) => type?.ToString("dd.MM.yyyy");

    internal static string ToRussianWithHours(this DateTime type) => type.ToString("dd.MM.yyyy HH:mm:ss");

    internal static string ToRussianWithHours(this DateTime? type) => type?.ToString("dd.MM.yyyy HH:mm:ss");

    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        var diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }

    public static int DayDifference(this DateTime start, DateTime date)
    {
        if (date > start)
        {
            return 0;
        }

        var result = (start - date).TotalDays;

        return result <= 0
            ? 0
            : Convert.ToInt32(result);
    }
}
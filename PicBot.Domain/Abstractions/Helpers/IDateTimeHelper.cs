namespace PicBot.Domain.Abstractions.Helpers;

public interface IDateTimeHelper
{
    /// <summary>
    /// Возвращает дату и время
    /// </summary>
    /// <returns></returns>
    DateTime GetLocalDateTimeNow();

    /// <summary>
    /// Возвращает дату и время по серверу telegram
    /// </summary>
    /// <returns></returns>
    DateTime GetUtcDateTimeNow();

    /// <summary>
    /// Возвращает дату
    /// </summary>
    /// <returns></returns>
    DateTime GetLocalDateNow();
}
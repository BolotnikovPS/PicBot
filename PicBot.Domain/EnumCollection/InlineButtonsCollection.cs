using System.Collections.Frozen;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.EnumCollection;

namespace PicBot.Domain.EnumCollection;

/// <summary>
/// Inline кнопки с расшифровками
/// </summary>
public class InlineButtonsCollection : CollectionBase<EInlineButtonsType>
{
    protected override FrozenDictionary<EInlineButtonsType, string> DataCollection { get; } = new Dictionary<EInlineButtonsType, string>
    {
        [EInlineButtonsType.ToClose] = "🚪 Закрыть",
        [EInlineButtonsType.ToBack] = "🔙 Назад",
        [EInlineButtonsType.GetActiveUsers] = "👍 Получить список активных пользователей",
        [EInlineButtonsType.GetNotActiveUsers] = "☠️ Получить список заблокированных пользователей",
        [EInlineButtonsType.ToLock] = "☠️ Заблокировать",
        [EInlineButtonsType.ToUnLock] = "🎉 Разблокировать",
        [EInlineButtonsType.Yes] = "❤️ Да",
        [EInlineButtonsType.UsersDetailedStatistics] = "Подробная статистика",
        [EInlineButtonsType.RefreshUserMenu] = "🔄 Обновить меню пользователя",
    }.ToFrozenDictionary();

    public static InlineButtonsCollection Instance { get; } = new();
}
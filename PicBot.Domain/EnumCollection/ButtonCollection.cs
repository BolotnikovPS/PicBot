using System.Collections.Frozen;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.EnumCollection;

namespace PicBot.Domain.EnumCollection;

/// <summary>
/// Кнопки с расшифровками
/// </summary>
public class ButtonCollection : CollectionBase<EButtonsType>
{
    protected override FrozenDictionary<EButtonsType, string> DataCollection { get; } = new Dictionary<EButtonsType, string>
    {
        [EButtonsType.ToBack] = "👈 Назад",
        [EButtonsType.ToBackMain] = "🏠 На главный экран",
        [EButtonsType.Admin] = "🔑 Админка",
        [EButtonsType.ListJobs] = "🗄 Список джобов",
        [EButtonsType.GetUsersStatistic] = "📊 Получить статистику пользователей",
        [EButtonsType.GetAllUsers] = "🕵️‍♂️ Получить список пользователей",
        [EButtonsType.RefreshMenu] = "🔄 Обновить меню пользователей",
    }.ToFrozenDictionary();

    public static ButtonCollection Instance { get; } = new();
}
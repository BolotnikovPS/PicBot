using PicBot.Domain.Bots;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Abstractions;
using TBotPlatform.Contracts.Bots.Buttons;
using TBotPlatform.Contracts.Bots.Users;

namespace PicBot.Application.Bots.BotPlatform.MenuButton;

internal class AdminMenuButton : IMenuButton
{
    public Task<MainButtonMassiveList> GetMainButtonsAsync<T>(T user)
        where T : UserBase
    {
        var menu = new MainButtonMassiveList
        {
            new()
            {
                MainButtons =
                [
                    new MyMainButton(EButtonsType.GetAllUsers),
                    new MyMainButton(EButtonsType.GetUsersStatistic),
                ],
            },
            new()
            {
                MainButtons =
                [
                    new MyMainButton(EButtonsType.ListJobs),
                    new MyMainButton(EButtonsType.RefreshMenu),
                ],
            },
            new()
            {
                MainButtons =
                [
                    new MyMainButton(EButtonsType.ToBackMain),
                ],
            },
        };

        return Task.FromResult(menu);
    }
}
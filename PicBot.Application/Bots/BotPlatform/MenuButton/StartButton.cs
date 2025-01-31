using PicBot.Domain.Bots;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Abstractions;
using TBotPlatform.Contracts.Bots.Buttons;
using TBotPlatform.Contracts.Bots.Users;

namespace PicBot.Application.Bots.BotPlatform.MenuButton;

internal class StartButton : IMenuButton
{
    public Task<MainButtonMassiveList> GetMainButtonsAsync<T>(T user)
        where T : UserBase
    {
        var result = new MainButtonMassiveList();

        if (!user.IsAdmin())
        {
            return Task.FromResult(result);
        }

        var admin = new MainButtonMassive
        {
            MainButtons =
            [
                new MyMainButton(EButtonsType.Admin),
            ],
        };
        result.Add(admin);

        return Task.FromResult(result);
    }
}
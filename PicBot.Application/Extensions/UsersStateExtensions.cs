using PicBot.Domain.Bots;
using TBotPlatform.Contracts.Bots.Constant;
using TBotPlatform.Contracts.Bots.Markups;
using TBotPlatform.Contracts.Bots.Markups.InlineMarkups;
using TBotPlatform.Contracts.Bots.Pagination;
using PicBot.Domain.Enums;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Application.Bots.BotPlatform.States.AdminStates.UserStates;
using PicBot.Application.Bots.BotPlatform.States.MessageStates;

namespace PicBot.Application.Extensions;

internal static class UsersStateExtensions
{
    public static InlineMarkupMassiveList CreateAllUserButtons(this List<User> users, int currentPosition)
    {
        var buttons = CreateUserButtons(users, currentPosition, nameof(AllUsersState));

        var thirdButtons = new InlineMarkupList
        {
            new MyInlineMarkupState(EInlineButtonsType.GetActiveUsers, nameof(ActiveUsersState)),
            new MyInlineMarkupState(EInlineButtonsType.ToClose, nameof(MessageCloseState)),
        };

        buttons.Add(
            new()
            {
                InlineMarkups = thirdButtons,
                ButtonsPerRow = 1,
            });

        return buttons;
    }

    public static InlineMarkupMassiveList CreateActiveUserButtons(this List<User> users, int currentPosition)
    {
        var buttons = CreateUserButtons(users, currentPosition, nameof(ActiveUsersState));

        var thirdButtons = new InlineMarkupList
        {
            new MyInlineMarkupState(EInlineButtonsType.GetNotActiveUsers, nameof(NotActiveUsersState)),
            new MyInlineMarkupState(EInlineButtonsType.ToClose, nameof(MessageCloseState)),
        };

        buttons.Add(
            new()
            {
                InlineMarkups = thirdButtons,
                ButtonsPerRow = 1,
            });

        return buttons;
    }

    public static InlineMarkupMassiveList CreateNotActiveUserButtons(this List<User> users, int currentPosition)
    {
        var buttons = CreateUserButtons(users, currentPosition, nameof(NotActiveUsersState));

        var thirdButtons = new InlineMarkupList
        {
            new MyInlineMarkupState(EInlineButtonsType.GetActiveUsers, nameof(ActiveUsersState)),
            new MyInlineMarkupState(EInlineButtonsType.ToClose, nameof(MessageCloseState)),
        };

        buttons.Add(
            new()
            {
                InlineMarkups = thirdButtons,
                ButtonsPerRow = 1,
            });

        return buttons;
    }

    private static InlineMarkupMassiveList CreateUserButtons(List<User> users, int currentPosition, string paginationState)
    {
        const int step = PaginationsConstant.Step;

        var firstButtons = new InlineMarkupList();

        var paginationData = users.GetPaginationData(step, currentPosition);

        foreach (var data in paginationData.Values)
        {
            firstButtons.Add(new InlineMarkupState($"{data.UserName}: {data.FirstName} {data.LastName}", nameof(UserInfoState), data.Id.ToString()));
        }

        var secondButtons = new InlineMarkupList();
        if (paginationData.IsPrevious)
        {
            secondButtons.Add(new InlineMarkupState(PaginationsConstant.PreviousPage, paginationState, paginationData.PreviousValue));
        }

        if (paginationData.IsNext)
        {
            secondButtons.Add(new InlineMarkupState(PaginationsConstant.NextPage, paginationState, paginationData.NextValue));
        }

        return
        [
            new()
            {
                InlineMarkups = firstButtons,
                ButtonsPerRow = 1,
            },
            new()
            {
                InlineMarkups = secondButtons,
                ButtonsPerRow = 2,
            },
        ];
    }
}
#nullable enable
using MediatR;
using Microsoft.Extensions.Logging;
using PicBot.Application.CQ.DbContext.BotPlatformContext.Commands;
using PicBot.Application.Extensions;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform.Enums;
using PicBot.Domain.EnumCollection;
using PicBot.Domain.Enums;
using TBotPlatform.Common;
using TBotPlatform.Contracts;
using TBotPlatform.Contracts.Abstractions.Factories;
using TBotPlatform.Contracts.Abstractions.Handlers;
using TBotPlatform.Contracts.Bots;
using TBotPlatform.Contracts.Bots.ChatUpdate;
using TBotPlatform.Extension;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = PicBot.Domain.Contexts.BotPlatform.User;

namespace PicBot.Application.Bots;

internal sealed class StartReceivingHandler(
    ILogger<StartReceivingHandler> logger,
    IMediator mediator,
    IStateFactory stateFactory,
    IStateContextFactory stateContextFactory,
    IMenuButtonFactory menuButtonFactory,
    IBotType botType
    ) : IStartReceivingHandler
{
    public async Task HandleUpdate(Update chatUpdate, MarkupNextState? markupNextState, TelegramMessageUserData telegramUser, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        User user;
        try
        {
            user = await mediator.Send(new GetOrCreateUserCommand(telegramUser), cancellationToken);

            if (user.IsNull())
            {
                return;
            }

            if (user.IsLock() && chatUpdate.Type.NotIn(UpdateType.MyChatMember))
            {
                var stateHistory = stateFactory.LockState;
                await using var stateContext = await stateContextFactory.CreateStateContext(user, stateHistory, chatUpdate, cancellationToken);

                return;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка определения пользователя для запроса {update}", chatUpdate.ToJson());

            return;
        }

        if (chatUpdate.Type.In(UpdateType.Message, UpdateType.CallbackQuery))
        {
            await DoWorkMessageAsync(user, chatUpdate, markupNextState, cancellationToken);
            return;
        }

        if (chatUpdate.Type.In(UpdateType.MyChatMember))
        {
            await DoWorkMemberAsync(user, chatUpdate, cancellationToken);
        }

        if (chatUpdate.Type == UpdateType.InlineQuery)
        {
            await DoWorkImageAsync(user, chatUpdate, cancellationToken);
        }
    }

    private async Task DoWorkImageAsync(User user, Update chatMessage, CancellationToken cancellationToken)
    {
        var stateTypeCommand = CommandCollection.Instance.GetKeyByValue(chatMessage.InlineQuery?.Query);

        var command = stateTypeCommand != ECommandsType.None ? stateTypeCommand : ECommandsType.FindImage;

        var stateHistory = await stateFactory.GetStateByCommandsTypeOrDefault(user.ChatId, CommandCollection.Instance.GetValueByKey(command), cancellationToken);

        await using var stateContext = await stateContextFactory.CreateStateContext(user, stateHistory, chatMessage, cancellationToken);
    }

    private async Task DoWorkMessageAsync(User user, Update chatMessage, MarkupNextState? markupNextState, CancellationToken cancellationToken)
    {
        try
        {
            var stateHistory = chatMessage.Type switch
            {
                UpdateType.Message => await DetermineStateAsync(user.ChatId, chatMessage, stateFactory, cancellationToken),
                UpdateType.CallbackQuery => stateFactory.GetStateByNameOrDefault(markupNextState?.State),
                _ => null,
            };

            if (stateHistory.IsNull())
            {
                if (chatMessage.Message.ContainPhotoInMessage())
                {
                    stateHistory = await stateFactory.GetStateByCommandsTypeOrDefault(user.ChatId, CommandCollection.Instance.GetValueByKey(ECommandsType.SaveImage), cancellationToken);
                }
                else
                {
                    stateHistory = await stateFactory.GetStateMain(user.ChatId, cancellationToken);
                }
            }

            await using var stateContext = await stateContextFactory.CreateStateContext(user, stateHistory!, chatMessage, markupNextState, cancellationToken);

            var stateResult = stateContextFactory.GetStateResult(stateContext);

            if (stateHistory!.MenuStateTypeOrNull.IsNotNull() || stateResult!.IsNeedUpdateMarkup)
            {
                var markUp = stateResult!.IsNeedUpdateMarkup
                    ? await stateFactory.GetLastStateWithMenu(user.ChatId, cancellationToken)
                    : stateHistory;
                await menuButtonFactory.UpdateMainButtonsByState(user, stateContext, markUp, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Для пользователя {user} возникло исключение", user.ToJson());
        }
    }

    private async Task DoWorkMemberAsync(User user, Update chatMessage, CancellationToken cancellationToken)
    {
        try
        {
            var chatMember = chatMessage.MyChatMember?.NewChatMember;

            if (chatMember.IsNull())
            {
                return;
            }

            if (chatMember!.Status.In(ChatMemberStatus.Kicked))
            {
                var updateUserCommand = new UpdateUserCommand(user.Id, EUserBlockType.KickedByUser);
                await mediator.Send(updateUserCommand, cancellationToken);

                return;
            }

            if (chatMember.Status.In(ChatMemberStatus.Member))
            {
                var blockType = botType.GetBotType().BotSetting.WithRegistration
                    ? EUserBlockType.Registration
                    : EUserBlockType.None;

                var updateUserCommand = new UpdateUserCommand(user.Id, blockType);
                await mediator.Send(updateUserCommand, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Для пользователя {user} возникло исключение.", user.ToJson());
        }
    }

    private static async Task<StateHistory?> DetermineStateAsync(long chatId, Update chatMessage, IStateFactory stateFactory, CancellationToken cancellationToken)
    {
        var stateTypeText = TextCollection.Instance.GetKeyByValue(chatMessage.Message?.ReplyToMessage?.Text);
        var stateTypeButton = ButtonCollection.Instance.GetKeyByValue(chatMessage.Message?.Text);
        var stateTypeCommand = CommandCollection.Instance.GetKeyByValue(chatMessage.Message?.Text);

        if (stateTypeButton == EButtonsType.None
            && stateTypeText == ETextsType.None
            && stateTypeCommand == ECommandsType.None
           )
        {
            return await stateFactory.GetBindStateOrNull(chatId, cancellationToken);
        }

        if (stateTypeButton != EButtonsType.None)
        {
            return stateTypeButton switch
            {
                EButtonsType.ToBack => await stateFactory.GetStatePreviousOrMain(chatId, cancellationToken),
                EButtonsType.ToBackMain => await stateFactory.GetStateMain(chatId, cancellationToken),
                _ => await stateFactory.GetStateByButtonsTypeOrDefault(chatId, stateTypeButton.ToString(), cancellationToken),
            };
        }

        if (stateTypeText != ETextsType.None)
        {
            return stateFactory.GetStateByTextsTypeOrDefault(chatId, stateTypeText.ToString());
        }

        if (stateTypeCommand != ECommandsType.None)
        {
            return await stateFactory.GetStateByCommandsTypeOrDefault(chatId, chatMessage.Message?.Text, cancellationToken);
        }

        return null;
    }
}
using PicBot.Application.Attributes;
using PicBot.Application.Abstractions;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace PicBot.Application.Bots.BotPlatform.States.InlineStates;

[MyStateInlineActivator(CommandsTypes = [ECommandsType.FindImage,])]
internal class ImageFindState(IImageService imageService) : IMyState
{
    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var images = await imageService.GetImageByTextAsync(context.ChatUpdate.InlineQueryOrNull!.Query, cancellationToken);

        var i = 0;
        var inlineQueryResultPhotos = images
                                     .Select(
                                          z =>
                                          {
                                              i++;
                                              return new InlineQueryResultPhoto(i.ToString(), z.Url, z.ThumbUrl)
                                              {
                                                  ParseMode = ParseMode.Html,
                                              };
                                          })
                                     .ToList();

        var telegramContext = context.GetTelegramContext();

        await telegramContext.MakeRequestAsync(Request, cancellationToken);
        return;

        Task Request(ITelegramBotClient req)
        {
            return req.AnswerInlineQuery(context.ChatUpdate.InlineQueryOrNull!.Id, inlineQueryResultPhotos, cancellationToken: cancellationToken);
        }
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
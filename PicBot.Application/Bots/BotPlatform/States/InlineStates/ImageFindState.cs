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
    public async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var images = await imageService.GetImageByTextAsync(context.ChatUpdate.InlineQuery?.Query, cancellationToken);

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

        await context.TelegramContext.AnswerInlineQuery(context.ChatUpdate.InlineQuery!.Id, inlineQueryResultPhotos, cancellationToken: cancellationToken);
    }

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
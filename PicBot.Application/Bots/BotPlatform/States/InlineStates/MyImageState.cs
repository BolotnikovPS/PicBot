using MediatR;
using PicBot.Application.Attributes;
using PicBot.Application.CQ.DbContext.BotPlatformContext.Queries;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Extension;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace PicBot.Application.Bots.BotPlatform.States.InlineStates;

[MyStateInlineActivator(CommandsTypes = [ECommandsType.MyImage,])]
internal class MyImageState(IMediator mediator) : IMyState
{
    public async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        if (context.ChatUpdate.InlineQuery.IsNull())
        {
            return;
        }

        var request = new GetFilesQuery(user.Id);

        var files = await mediator.Send(request, cancellationToken);

        var inlineQueryResultPhotos = files
                                     .Select(
                                          z => new InlineQueryResultCachedPhoto(z.Id.ToString(), z.FileId)
                                          {
                                              ParseMode = ParseMode.Html,
                                          })
                                     .ToList();

        await context.TelegramContext.AnswerInlineQuery(context.ChatUpdate.InlineQuery!.Id, inlineQueryResultPhotos, cancellationToken: cancellationToken);
    }

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
using MediatR;
using PicBot.Application.Attributes;
using PicBot.Application.CQ.DbContext.BotPlatformContext.Commands;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Enums;
using TBotPlatform.Common;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace PicBot.Application.Bots.BotPlatform.States.InlineStates;

[MyStateInlineActivator(CommandsTypes = [ECommandsType.SaveImage,])]
internal class SaveImageFindState(IMediator mediator) : IMyState
{
    public async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        if (context.ChatUpdate.Message.ContainPhotoInMessage())
        {
            return;
        }

        var photo = await context.TryDownloadPhoto(context.ChatUpdate.Message, cancellationToken);
        var request = new AddFileCommand(user.Id, photo.FileId);

        await mediator.Send(request, cancellationToken);
    }

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
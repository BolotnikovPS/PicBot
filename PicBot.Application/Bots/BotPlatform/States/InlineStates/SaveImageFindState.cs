using MediatR;
using PicBot.Application.Attributes;
using PicBot.Application.CQ.DbContext.BotPlatformContext.Commands;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Extension;

namespace PicBot.Application.Bots.BotPlatform.States.InlineStates;

[MyStateInlineActivator(CommandsTypes = [ECommandsType.SaveImage,])]
internal class SaveImageFindState(IMediator mediator) : IMyState
{
    public Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        if (context.ChatUpdate.Message.PhotoDataOrNull.IsNull())
        {
            return Task.CompletedTask;
        }

        var request = new AddFileCommand(user.Id, context.ChatUpdate.Message.PhotoDataOrNull!.FileId);

        return mediator.Send(request, cancellationToken);
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
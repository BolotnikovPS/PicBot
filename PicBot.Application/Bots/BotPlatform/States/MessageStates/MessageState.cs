using System.Text;
using PicBot.Application.Attributes;
using PicBot.Application.Bots.BotPlatform.MenuButton;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace PicBot.Application.Bots.BotPlatform.States.MessageStates;

[MyStateActivator(typeof(StartButton), CommandsTypes = [ECommandsType.Start,])]
internal class MessageState : IMyState
{
    public async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await context.SendTextMessage(
            "Добро пожаловать на борт, добрый путник!",
            cancellationToken
            );

        var sbMessage = new StringBuilder()
                       .AppendLine("Команды в строке поиска:")
                       .AppendLine("    /my - мои ранее отправленные картинки.");

        await context.SendTextMessage(sbMessage.ToString(), cancellationToken);
    }

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
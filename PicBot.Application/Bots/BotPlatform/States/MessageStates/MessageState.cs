using System.Text;
using PicBot.Application.Attributes;
using PicBot.Domain.Abstractions.BotControl;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace PicBot.Application.Bots.BotPlatform.States.MessageStates;

[MyStateInlineActivator(CommandsTypes = [ECommandsType.Start,])]
internal class MessageState : IMyState
{
    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await context.SendTextMessageAsync(
            "Добро пожаловать на борт, добрый путник!",
            cancellationToken
            );

        var sbMessage = new StringBuilder()
                       .AppendLine("Команды в строке поиска:")
                       .AppendLine("    /my - мои ранее отправленные картинки.");

        await context.SendTextMessageAsync(sbMessage.ToString(), cancellationToken);
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Application.Abstractions.DBContext;
using PicBot.Domain.Abstractions.CQRS.Command;
using PicBot.Domain.Abstractions.Helpers;

namespace PicBot.Application.CQ.DbContext.BotPlatformContext.Commands;

internal record AddFileCommand(int UserId, string FileId) : ICommand<int>;

internal class AddFileCommandHandler(IBotPlatformDbContext tgBotDbContext, IDateTimeHelper dateTimeHelper)
    : ICommandHandler<AddFileCommand, int>
{
    public async Task<int> Handle(AddFileCommand request, CancellationToken cancellationToken)
    {
        var file = new FileBox
        {
            UserId = request.UserId,
            FileId = request.FileId,
            Create = dateTimeHelper.GetLocalDateNow(),
        };

        await tgBotDbContext.FilesBox.AddAsync(file, cancellationToken);
        await tgBotDbContext.SaveChangesAsync(cancellationToken);

        return file.Id;
    }
}
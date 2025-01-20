using Microsoft.EntityFrameworkCore;
using PicBot.Application.Abstractions.DBContext;
using PicBot.Domain.Abstractions.CQRS.Command;
using PicBot.Domain.Abstractions.Helpers;
using TBotPlatform.Extension;

namespace PicBot.Application.CQ.DbContext.BotPlatformContext.Commands;

internal record AddFileCommand(int UserId, string FileId) : ICommand<int>;

internal class AddFileCommandHandler(IBotPlatformDbContext tgBotDbContext, IDateTimeHelper dateTimeHelper)
    : ICommandHandler<AddFileCommand, int>
{
    public async Task<int> Handle(AddFileCommand request, CancellationToken cancellationToken)
    {
        var file = await tgBotDbContext.FilesBox.FirstOrDefaultAsync(z => z.FileId == request.FileId, cancellationToken);

        if (file.IsNotNull())
        {
            return file.Id;
        }

        file = new()
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
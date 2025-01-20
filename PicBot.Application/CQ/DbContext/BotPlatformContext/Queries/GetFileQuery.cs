using Microsoft.EntityFrameworkCore;
using PicBot.Application.Abstractions.DBContext;
using PicBot.Domain.Abstractions.CQRS.Query;
using PicBot.Domain.Contexts.BotPlatform;

namespace PicBot.Application.CQ.DbContext.BotPlatformContext.Queries;

internal record GetFileQuery(int FileId) : IQuery<FileBox>;

internal class GetFileQueryHandler(IBotPlatformDbContext tgBotDbContext)
    : IQueryHandler<GetFileQuery, FileBox>
{
    public Task<FileBox> Handle(GetFileQuery request, CancellationToken cancellationToken)
        => tgBotDbContext.FilesBox.FirstOrDefaultAsync(z => z.Id == request.FileId, cancellationToken);
}
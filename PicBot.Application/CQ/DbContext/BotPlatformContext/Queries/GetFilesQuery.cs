using Microsoft.EntityFrameworkCore;
using PicBot.Application.Abstractions.DBContext;
using PicBot.Domain.Abstractions.CQRS.Query;
using PicBot.Domain.Contexts.BotPlatform;

namespace PicBot.Application.CQ.DbContext.BotPlatformContext.Queries;

internal record GetFilesQuery(int UserId) : IQuery<List<FileBox>>;

internal class GetFilesQueryHandler(IBotPlatformDbContext tgBotDbContext)
    : IQueryHandler<GetFilesQuery, List<FileBox>>
{
    public Task<List<FileBox>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
        => tgBotDbContext.FilesBox.Where(z => z.UserId == request.UserId).ToListAsync(cancellationToken);
}
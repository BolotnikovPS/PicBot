using Microsoft.EntityFrameworkCore;
using PicBot.Application.Abstractions.DBContext;
using PicBot.Domain.Abstractions.CQRS.Query;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Contexts.BotPlatform.Enums;
using TBotPlatform.Extension;

namespace PicBot.Application.CQ.DbContext.BotPlatformContext.Queries;

internal record UsersQuery(string UserName, long? UserId, EUserBlockType? BlockType) : IQuery<List<User>>
{
    public UsersQuery()
        : this(null, null, null)
    {
    }
}

internal class UsersQueryHandler(
    IBotPlatformDbContext dbContext
    ) : IQueryHandler<UsersQuery, List<User>>
{
    public Task<List<User>> Handle(UsersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<User> users = dbContext.Users;

        if (request.UserId.IsNotNull())
        {
            users = users.Where(z => z.Id == request.UserId);
        }

        if (request.UserName.CheckAny())
        {
            users = users.Where(z => z.UserName == request.UserName);
        }

        if (request.BlockType.IsNotNull())
        {
            users = users.Where(z => z.BlockType == request.BlockType);
        }

        return users.ToListAsync(cancellationToken);
    }
}
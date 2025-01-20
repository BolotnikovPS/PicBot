using Microsoft.EntityFrameworkCore;
using PicBot.Application.Abstractions.DBContext;
using PicBot.Domain.Abstractions.CQRS.Query;
using PicBot.Domain.Contexts.BotPlatform;
using TBotPlatform.Extension;

namespace PicBot.Application.CQ.DbContext.BotPlatformContext.Queries;

internal record UserQuery(string UserName, long? TgUserId, long? UserId) : IQuery<User>;

internal class UserQueryHandler(
    IBotPlatformDbContext dbContext
    ) : IQueryHandler<UserQuery, User>
{
    public Task<User> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        IQueryable<User> users = dbContext.Users;

        if (request.UserName.CheckAny())
        {
            users = users.Where(z => z.UserName == request.UserName);
        }

        if (request.TgUserId.IsNotNull())
        {
            users = users.Where(z => z.TgUserId.ToString() == request.TgUserId.ToString());
        }

        if (request.UserId.IsNotNull())
        {
            users = users.Where(z => z.Id.ToString() == request.UserId.ToString());
        }

        return users.FirstOrDefaultAsync(cancellationToken);
    }
}
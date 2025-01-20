using Microsoft.EntityFrameworkCore;
using PicBot.Application.Abstractions.DBContext;
using PicBot.Domain.Abstractions.CQRS.Query;
using PicBot.Domain.Contexts.BotPlatform;

namespace PicBot.Application.CQ.DbContext.BotPlatformContext.Queries;

internal record VerificationQuery(int UserId) : IQuery<Verification>;

internal class VerificationQueryHandler(
    IBotPlatformDbContext tgBotDbContext
    ) : IQueryHandler<VerificationQuery, Verification>
{
    public async Task<Verification> Handle(VerificationQuery request, CancellationToken cancellationToken)
    {
        return await tgBotDbContext.Verifications.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
    }
}
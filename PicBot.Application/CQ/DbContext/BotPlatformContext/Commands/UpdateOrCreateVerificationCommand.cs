using Microsoft.EntityFrameworkCore;
using PicBot.Application.Abstractions.DBContext;
using PicBot.Domain.Abstractions.CQRS.Command;
using PicBot.Domain.Contexts.BotPlatform.Enums;
using TBotPlatform.Extension;

namespace PicBot.Application.CQ.DbContext.BotPlatformContext.Commands;

internal record UpdateOrCreateVerificationCommand(int UserId, EUserEventType? EventType) : ICommand;

internal class UpdateOrCreateVerificationCommandHandler(
    IBotPlatformDbContext tgBotDbContext
    ) : ICommandHandler<UpdateOrCreateVerificationCommand>
{
    public async Task Handle(UpdateOrCreateVerificationCommand request, CancellationToken cancellationToken)
    {
        var result = await tgBotDbContext.Verifications.FirstOrDefaultAsync(z => z.UserId == request.UserId, cancellationToken);

        if (result.IsNull())
        {
            result = new()
            {
                UserId = request.UserId,
            };

            await tgBotDbContext.Verifications.AddAsync(result, cancellationToken);
            await tgBotDbContext.SaveChangesAsync(cancellationToken);
        }

        if (request.EventType.HasValue)
        {
            result.EventType = request.EventType;
        }

        tgBotDbContext.Verifications.Update(result);
        await tgBotDbContext.SaveChangesAsync(cancellationToken);
    }
}
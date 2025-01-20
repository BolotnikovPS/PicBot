using Microsoft.EntityFrameworkCore;
using PicBot.Application.Abstractions.DBContext;
using PicBot.Domain.Abstractions.CQRS.Command;
using PicBot.Domain.Contexts.BotPlatform.Enums;
using TBotPlatform.Extension;

namespace PicBot.Application.CQ.DbContext.BotPlatformContext.Commands;

internal record UpdateUserCommand(long UserId, EUserBlockType? BlockType) : ICommand;

internal class UpdateUserCommandHandler(
    IBotPlatformDbContext tgBotDbContext
    ) : ICommandHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await tgBotDbContext.Users.FirstOrDefaultAsync(z => z.Id == request.UserId, cancellationToken);

        if (result.IsNull())
        {
            throw new($"Пользователь с UserId {request.UserId} не найден");
        }

        if (request.BlockType.HasValue)
        {
            result.BlockType = request.BlockType;
        }

        tgBotDbContext.Users.Update(result);
        await tgBotDbContext.SaveChangesAsync(cancellationToken);
    }
}
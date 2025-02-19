using PicBot.Application.CQ.DbContext.BotPlatformContext.Queries;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Factories;
using PicBot.Domain.Contexts.BotPlatform.Enums;
using PicBot.Domain.Abstractions.Publishers.EventDomain;
using PicBot.Application.Contracts.Messages.DomainMessage;

namespace PicBot.Application.Handlers.EventDomain;

internal class RefreshMenuMessageHandler(IMediator mediator, IStateContextFactory stateContextFactory, IStateFactory stateFactory, IMenuButtonFactory menuButtonFactory) : IEventDomainMessageHandler<RefreshMenuMessage>
{
    public async Task Handle(RefreshMenuMessage message, CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new UsersQuery(null, message.UserId, EUserBlockType.None), cancellationToken);

        foreach (var user in users)
        {
            var state = stateFactory.GetStateByNameOrDefault();
            await using var stateContext = stateContextFactory.GetStateContext(user);

            await menuButtonFactory.UpdateMainButtonsByState(user, stateContext, state, cancellationToken);
        }
    }
}
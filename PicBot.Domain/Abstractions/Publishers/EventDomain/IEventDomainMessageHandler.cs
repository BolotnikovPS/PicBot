using MediatR;

namespace PicBot.Domain.Abstractions.Publishers.EventDomain;

public interface IEventDomainMessageHandler<in TMessage>
    : INotificationHandler<TMessage>
    where TMessage : IEventDomainMessage
{
    new Task Handle(TMessage message, CancellationToken cancellationToken);
}
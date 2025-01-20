using PicBot.Domain.Abstractions.Publishers.EventDomain;

namespace PicBot.Application.Contracts.Messages.DomainMessage;

internal record RefreshMenuMessage(int? UserId = null) : IEventDomainMessage;
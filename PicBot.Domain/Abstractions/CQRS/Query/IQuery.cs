using MediatR;

namespace PicBot.Domain.Abstractions.CQRS.Query;

public interface IQuery<out TResponse>
    : IRequest<TResponse>;
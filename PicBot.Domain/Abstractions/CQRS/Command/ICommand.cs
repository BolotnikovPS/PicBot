using MediatR;

namespace PicBot.Domain.Abstractions.CQRS.Command;

public interface ICommand
    : IRequest;

public interface ICommand<out TResponse>
    : IRequest<TResponse>;
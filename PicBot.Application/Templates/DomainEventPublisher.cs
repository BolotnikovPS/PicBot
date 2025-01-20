using MediatR;
using Microsoft.Extensions.Logging;
using PicBot.Domain.Abstractions.Publishers.EventDomain;
using System.Diagnostics;
using System.Text;
using TBotPlatform.Extension;

namespace PicBot.Application.Templates;

internal class DomainEventPublisher(
    ILogger<DomainEventPublisher> logger,
    IPublisher publisher
    ) : IEventDomainPublisher
{
    async Task IEventDomainPublisher.PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Exception exception = null;
        var sbLog = new StringBuilder();
        var sw = new Stopwatch();

        try
        {
            sbLog.AppendLine($"Сообщение типа {message.GetType().FullName}: {message.ToJson()}");

            await publisher.Publish(message, cancellationToken);
        }
        catch (Exception ex)
        {
            exception = ex;
            throw;
        }
        finally
        {
            sw.Stop();

            sbLog.AppendLine($"Время выполнения {sw.Elapsed.Milliseconds} милли секунд.");

            var logLevel = exception.IsNotNull() ? LogLevel.Error : LogLevel.Debug;
            logger.Log(logLevel, exception, sbLog.ToString());
        }
    }
}
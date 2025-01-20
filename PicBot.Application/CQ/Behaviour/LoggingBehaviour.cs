using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;
using TBotPlatform.Extension;

namespace PicBot.Application.CQ.Behaviour
{
    internal class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Exception exception = null;
            var sbLog = new StringBuilder();
            var sw = new Stopwatch();

            try
            {
                sbLog.AppendLine($"Handling {typeof(TRequest).Name}");

                var myType = request.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                foreach (var prop in props)
                {
                    var propValue = prop.GetValue(request, null);
                    sbLog.AppendLine($"{prop.Name} : {propValue}");
                }

                sw.Start();
                var response = await next();

                sbLog.AppendLine($"Handled {typeof(TResponse).Name}: {response.ToJson()}");

                return response;
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
}

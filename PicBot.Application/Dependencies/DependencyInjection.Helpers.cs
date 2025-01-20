using Microsoft.Extensions.DependencyInjection;
using PicBot.Application.Helpers;
using PicBot.Domain.Abstractions.Helpers;

namespace PicBot.Application.Dependencies;

public static partial class DependencyInjection
{
    internal static IServiceCollection AddHelpers(this IServiceCollection services)
        => services
          .AddSingleton<IDeclensionHelper, DeclensionHelper>()
          .AddSingleton<IDateTimeHelper, DateTimeHelper>();
}
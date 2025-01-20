using PicBot.Domain.Bots.Config;

namespace PicBot.Application.Abstractions;

public interface IConfigService
{
    T GetValueOrNull<T>(EConfigKey key);

    string GetValueOrNull(EConfigKey key);
}
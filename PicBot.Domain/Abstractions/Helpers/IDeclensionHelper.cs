using PicBot.Domain.Enums;

namespace PicBot.Domain.Abstractions.Helpers;

public interface IDeclensionHelper
{
    string Decline(int value, EDeclensionType type);
}
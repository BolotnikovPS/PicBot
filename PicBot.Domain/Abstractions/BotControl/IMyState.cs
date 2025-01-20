using PicBot.Domain.Contexts.BotPlatform;
using TBotPlatform.Contracts.Abstractions.State;

namespace PicBot.Domain.Abstractions.BotControl;

public interface IMyState : IState<User>;
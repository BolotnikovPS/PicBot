using PicBot.Domain.EnumCollection;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Bots.Buttons;

namespace PicBot.Domain.Bots;

public class MyMainButton(EButtonsType button) : MainButton(ButtonCollection.Instance.GetValueByKey(button));
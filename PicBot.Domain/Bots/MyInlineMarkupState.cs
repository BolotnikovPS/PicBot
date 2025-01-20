using PicBot.Domain.EnumCollection;
using PicBot.Domain.Enums;
using TBotPlatform.Contracts.Bots.Markups.InlineMarkups;

namespace PicBot.Domain.Bots;

public class MyInlineMarkupState(EInlineButtonsType buttonName, string state = null, string data = null)
    : InlineMarkupState(InlineButtonsCollection.Instance.GetValueByKey(buttonName), state, data);
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBotfromArtyProf.Configuration;
using TelegramBotfromArtyProf.Interfaces;

namespace TelegramBotfromArtyProf.Services;

public class CurrencyHandler : ICurrencyHandler
{
    private readonly CurrencyExchangeConfiguration _currencyExchangeConfiguration;

    public CurrencyHandler(CurrencyExchangeConfiguration currencyExchangeConfiguration)
    {
        _currencyExchangeConfiguration = currencyExchangeConfiguration;
    }

    public async Task<Message> SendCurrencyExchange(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {

        return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Greeting! api key: {_currencyExchangeConfiguration.CurrencyExchangeKey}",
                cancellationToken: cancellationToken);
    }
}

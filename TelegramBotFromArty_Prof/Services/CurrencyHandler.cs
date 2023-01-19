using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBotfromArtyProf.Configuration;

namespace TelegramBotfromArtyProf.Services;

public class CurrencyHandler
{
    private readonly CurrencyExchangeConfiguration _currencyExchangeConfiguration;

    public CurrencyHandler(CurrencyExchangeConfiguration currencyExchangeConfiguration)
    {
        _currencyExchangeConfiguration = currencyExchangeConfiguration;
    }

    public async Task<Message> SendGreetings(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {

        return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Greeting! api key: {_currencyExchangeConfiguration.CurrencyExchangeKey}",
                cancellationToken: cancellationToken);
    }
}

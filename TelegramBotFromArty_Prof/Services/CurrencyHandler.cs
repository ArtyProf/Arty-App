using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBotfromArtyProf.Configuration;
using TelegramBotfromArtyProf.Interfaces;
using Microsoft.Extensions.Options;

namespace TelegramBotfromArtyProf.Services;

public class CurrencyHandler : ICurrencyHandler
{
    private readonly CurrencyExchangeConfiguration _currencyExchangeConfiguration;

    public CurrencyHandler(IOptions<CurrencyExchangeConfiguration> currencyExchangeOptions)
    {
        _currencyExchangeConfiguration = currencyExchangeOptions.Value;
    }

    public async Task<Message> SendCurrencyExchange(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {

        return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Greeting! api key: {_currencyExchangeConfiguration.CurrencyExchangeKey}",
                cancellationToken: cancellationToken);
    }
}

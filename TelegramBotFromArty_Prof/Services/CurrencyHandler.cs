using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBotfromArtyProf.Configuration;
using TelegramBotfromArtyProf.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace TelegramBotfromArtyProf.Services;

public class CurrencyHandler : ICurrencyHandler
{
    private readonly CurrencyExchangeConfiguration _currencyExchangeConfiguration;
    private readonly ILogger<CurrencyHandler> _logger;

    public CurrencyHandler(IOptions<CurrencyExchangeConfiguration> currencyExchangeOptions,
        ILogger<CurrencyHandler> logger)
    {
        _currencyExchangeConfiguration = currencyExchangeOptions.Value;
        _logger = logger;
    }

    public async Task<Message> SendCurrencyExchange(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Currency request started.");
        var messageText = message.Text ?? throw new ArgumentNullException();

        if(messageText.Split(new char[] { ' ', '@' }).Length < 4)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Wrong currency format.\nSee an example: /currency USD EUR 10",
                cancellationToken: cancellationToken);
        }

        var from = messageText.Split(new char[] { ' ', '@' })[1];
        var to = messageText.Split(new char[] { ' ', '@' })[2];
        var amount = messageText.Split(new char[] { ' ', '@' })[3];

        if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || string.IsNullOrEmpty(amount))
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Currency from and/or to and/or amount are not set.\nSee an example: /currency USD EUR 10",
                cancellationToken: cancellationToken);
        }
        var client = new RestClient($"https://api.apilayer.com/exchangerates_data/convert?to={to}&from={from}&amount={amount}");

        var request = new RestRequest();
        request.AddHeader("apikey", _currencyExchangeConfiguration.CurrencyExchangeKey);

        var response = client.Execute(request);
        int rate;
        int result;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);
            rate = responseBody?.info?.rate;
            result = responseBody?.result;

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Currency rate for today from {from} to {to}:\n" +
                $"Rate: {rate}\n" +
                $"Result: {result}",
                cancellationToken: cancellationToken);
        }

        return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Currency API is down.",
                cancellationToken: cancellationToken);
    }
}

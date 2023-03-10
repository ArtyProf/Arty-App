using Telegram.Bot.Types;
using Telegram.Bot;
using ArtyApp.Configuration;
using ArtyApp.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using ArtyApp.Helpers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using RestSharp;

namespace ArtyApp.Handlers;

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
        var messageText = message.Text;
        
        if (messageText.Split(' ').Length < 4)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Wrong currency format.\nSee an example: /currency USD EUR 10",
                cancellationToken: cancellationToken);
        }

        var from = messageText.Split(' ')[1];
        var to = messageText.Split(' ')[2];
        var amount = messageText.Split(' ')[3];

        if (!CurrencyHelper.TryGetCurrencySymbol(from, out var fromSymbol) || !CurrencyHelper.TryGetCurrencySymbol(to, out var toSymbol))
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Currency from and/or to are invalid.\nSee an example: /currency USD EUR 10",
                cancellationToken: cancellationToken);
        }
        var client = new RestClient($"https://api.apilayer.com/exchangerates_data/convert?to={to}&from={from}&amount={amount}");

        var request = new RestRequest();
        request.AddHeader("apikey", _currencyExchangeConfiguration.CurrencyExchangeKey);

        var response = client.Execute(request, cancellationToken);
        float rate;
        float result;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);
            rate = responseBody?.info?.rate;
            result = responseBody?.result;

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Currency rate for today from {from} {fromSymbol} to {to} {toSymbol}:\n" +
                $"Rate: {rate}\n" +
                $"Result: {result} {toSymbol}",
                cancellationToken: cancellationToken);
        }

        return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Currency API is down.",
                cancellationToken: cancellationToken);
    }
}

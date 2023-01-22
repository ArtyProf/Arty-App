using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telegram.Bot;
using TelegramBot.Configuration;
using TelegramBot.Interfaces;
using TelegramBot.Services;

[assembly: FunctionsStartup(typeof(TelegramBot.Startup))]
namespace TelegramBot;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddScoped<IBotBaseHandler, BotBaseHandler>();
        builder.Services.AddScoped<ICurrencyHandler, CurrencyHandler>();

        builder.Services.Configure<CurrencyExchangeConfiguration>(builder.GetContext().Configuration.GetSection(nameof(CurrencyExchangeConfiguration)));
        builder.Services.Configure<BotConfiguration>(builder.GetContext().Configuration.GetSection(nameof(BotConfiguration)));

        builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    TelegramBotClientOptions options = new(Environment.GetEnvironmentVariable($"{nameof(BotConfiguration)}__{nameof(BotConfiguration.BotToken)}"));
                    return new TelegramBotClient(options, httpClient);
                });

        builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
        var sgfd = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
        var res = sgfd.Where(x => x>7).FirstOrDefault();
    }
}
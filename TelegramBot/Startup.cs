using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using Telegram.Bot;
using TelegramBot.Configuration;
using TelegramBot.Controllers;
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

        var serviceProvider = builder.Services.BuildServiceProvider();

        var botConfiguration = serviceProvider.GetRequiredService<IOptions<BotConfiguration>>().Value;
        var bot = new TelegramBotClient(botConfiguration.BotToken);
        var webhookUrl = $"{botConfiguration.HostAddress}/api/{nameof(BotFunction)}";
        bot.SetWebhookAsync(webhookUrl);
    }
}
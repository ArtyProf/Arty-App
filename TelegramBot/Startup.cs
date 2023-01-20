using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot;

[assembly: FunctionsStartup(typeof(TelegramBot.Startup))]
namespace TelegramBot;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var currencyConfiguration = StartupHelper.GetConfiguredCurrencyExchangeConfiguration();
        var botConfiguration = StartupHelper.GetConfiguredBotConfiguration();

        builder.Services.AddSingleton(currencyConfiguration);
        builder.Services.AddSingleton(botConfiguration);
    }
}
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Telegram.Bot;
using TelegramBot.Configuration;
using TelegramBot.Controllers;

[assembly: FunctionsStartup(typeof(TelegramBot.Startup))]
namespace TelegramBot;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddOptions<CurrencyExchangeConfiguration>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection("CurrencyExchangeConfiguration").Bind(settings);
        });
        var botConfiguration = StartupHelper.GetConfiguredBotConfiguration();

        /*var bot = new TelegramBotClient(botConfiguration.BotToken);
        var webhookUrl = $"{botConfiguration.HostAddress}/api/{nameof(BotFunction)}";
        bot.SetWebhookAsync(webhookUrl);*/

        builder.Services.AddSingleton(botConfiguration);
        var serviceProvider = builder.Services.BuildServiceProvider();
        var config = serviceProvider.GetService<IOptions<CurrencyExchangeConfiguration>>();
    }
}
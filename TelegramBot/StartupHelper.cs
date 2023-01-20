using System;
using TelegramBot.Configuration;

namespace TelegramBot;

public static class StartupHelper
{
    public static CurrencyExchangeConfiguration GetConfiguredCurrencyExchangeConfiguration()
    {
        return new CurrencyExchangeConfiguration
        {
            CurrencyExchangeKey = Environment.GetEnvironmentVariable(
                $"{nameof(CurrencyExchangeConfiguration)}__{nameof(CurrencyExchangeConfiguration.CurrencyExchangeKey)}")
        };
    }

    public static BotConfiguration GetConfiguredBotConfiguration()
    {
        return new BotConfiguration
        {
            BotToken = Environment.GetEnvironmentVariable(nameof(BotConfiguration.BotToken)),
            HostAddress = Environment.GetEnvironmentVariable(nameof(BotConfiguration.HostAddress)),
            Route = Environment.GetEnvironmentVariable(nameof(BotConfiguration.Route))
        };
    }
}

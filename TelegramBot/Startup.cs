using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using OpenAI_API;
using System;
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
        var configuration = builder.GetContext().Configuration;
        builder.Services.Configure<CurrencyExchangeConfiguration>(configuration.GetSection(nameof(CurrencyExchangeConfiguration)));
        builder.Services.Configure<BotConfiguration>(configuration.GetSection(nameof(BotConfiguration)));

        builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    TelegramBotClientOptions options = new(Environment.GetEnvironmentVariable($"{nameof(BotConfiguration)}__{nameof(BotConfiguration.BotToken)}"));
                    return new TelegramBotClient(options, httpClient);
                });

        var openaiKey = Environment.GetEnvironmentVariable(nameof(OpenAIConfiguration.OpenAIKey));
        builder.Services.AddSingleton(s =>
        {
            return new OpenAIAPI(openaiKey);
        });

        builder.Services.AddScoped<IBotBaseHandler, BotBaseHandler>();
        builder.Services.AddScoped<ICurrencyHandler, CurrencyHandler>();
        builder.Services.AddScoped<IQuestionHandler, QuestionHandler>();

        builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
    }
}
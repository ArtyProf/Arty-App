using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Telegram.Bot;
using TelegramBot.Configuration;
using TelegramBot.Interfaces;
using TelegramBot.Handlers;
using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3;

[assembly: FunctionsStartup(typeof(TelegramBot.Startup))]
namespace TelegramBot;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.GetContext().Configuration;
        builder.Services.Configure<CurrencyExchangeConfiguration>(configuration.GetSection(nameof(CurrencyExchangeConfiguration)));
        builder.Services.Configure<BotConfiguration>(configuration.GetSection(nameof(BotConfiguration)));
        builder.Services.Configure<OpenAIConfiguration>(configuration.GetSection(nameof(OpenAIConfiguration)));

        builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    TelegramBotClientOptions options = new(Environment.GetEnvironmentVariable($"{nameof(BotConfiguration)}__{nameof(BotConfiguration.BotToken)}"));
                    return new TelegramBotClient(options, httpClient);
                });

        builder.Services.AddScoped<IBotBaseHandler, BotBaseHandler>();
        builder.Services.AddScoped<ICurrencyHandler, CurrencyHandler>();
        builder.Services.AddScoped<IQuestionHandler, QuestionHandler>();
        builder.Services.AddScoped<IImageHandler, ImageHandler>();

        builder.AddSwashBuckle(Assembly.GetExecutingAssembly());

        builder.Services.AddOpenAIService(settings => 
        { 
            settings.ApiKey = Environment.GetEnvironmentVariable($"{nameof(OpenAIConfiguration)}__{nameof(OpenAIConfiguration.OpenAIKey)}"); 
        });

    }
}
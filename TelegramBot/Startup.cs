using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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

        builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
    }
}
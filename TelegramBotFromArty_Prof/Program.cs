using Telegram.Bot;
using TelegramBotfromArtyProf;
using TelegramBotfromArtyProf.Configuration;
using TelegramBotfromArtyProf.Controllers;
using TelegramBotfromArtyProf.Interfaces;
using TelegramBotfromArtyProf.Services;

var builder = WebApplication.CreateBuilder(args);


var botConfigurationSection = builder.Configuration.GetSection(nameof(BotConfiguration));
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>() ?? throw new ArgumentNullException(nameof(BotConfiguration));

var currencyExchangeConfigation = builder.Configuration.GetSection(nameof(CurrencyExchangeConfiguration));
builder.Services.Configure<CurrencyExchangeConfiguration>(currencyExchangeConfigation);

builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                    TelegramBotClientOptions options = new(botConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

builder.Services.AddScoped<BotBaseHandlers>();
builder.Services.AddScoped<ICurrencyHandler, CurrencyHandler>();

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddControllers().AddNewtonsoftJson();

// Setup Services
var app = builder.Build();

// Map APIs
app.MapBotWebhookRoute<BotController>(route: botConfiguration.Route);
app.MapControllers();
app.MapGet("/", () => $"Bot is running");

// Start the Server
app.Run();

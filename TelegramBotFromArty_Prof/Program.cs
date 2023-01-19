using Telegram.Bot;
using TelegramBotfromArtyProf;
using TelegramBotfromArtyProf.Configuration;
using TelegramBotfromArtyProf.Controllers;
using TelegramBotfromArtyProf.Services;

var builder = WebApplication.CreateBuilder(args);


var botConfigurationSection = builder.Configuration.GetSection(nameof(BotConfiguration));
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>() ?? throw new ArgumentNullException(nameof(BotConfiguration));


builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                    TelegramBotClientOptions options = new(botConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

builder.Services.AddScoped<UpdateHandlers>();

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddControllers().AddNewtonsoftJson();

// Setup Services
var app = builder.Build();

// Map APIs
app.MapBotWebhookRoute<BotController>(route: botConfiguration.Route);
app.MapControllers();

// Start the Server
app.Run();

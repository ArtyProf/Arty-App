using Telegram.Bot;
using TelegramBotfromArtyProf.Configuration;

var builder = WebApplication.CreateBuilder(args);


var botConfigurationSection = builder.Configuration.GetSection(nameof(BotConfiguration));
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>() ?? throw new ArgumentNullException(nameof(BotConfiguration));


builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient) =>
                {
                    TelegramBotClientOptions options = new(botConfiguration.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

builder.Services.AddControllers().AddNewtonsoftJson();

// Setup Services
var app = builder.Build();

// Map APIs
app.MapControllers();
app.MapGet("/", () => $"Hello World! I am deployed! See token: {botConfiguration.BotToken}");

// Start the Server
app.Run();

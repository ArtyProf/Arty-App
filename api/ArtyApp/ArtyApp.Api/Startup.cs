using ArtyApp.Configuration;
using ArtyApp.Handlers;
using ArtyApp.Interfaces;
using Microsoft.OpenApi.Models;
using OpenAI.GPT3.Extensions;
using System.Reflection;
using Telegram.Bot;

namespace ArtyApp;

/// <summary>
/// Start up for the application middleware
/// </summary>
public class Startup
{
    /// <summary>
    /// Constructor for the application middleware
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Application Configuration
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<CurrencyExchangeConfiguration>(Configuration.GetSection(nameof(CurrencyExchangeConfiguration)));
        services.Configure<BotConfiguration>(Configuration.GetSection(nameof(BotConfiguration)));
        services.Configure<OpenAIConfiguration>(Configuration.GetSection(nameof(OpenAIConfiguration)));

        services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    TelegramBotClientOptions options = new(Environment.GetEnvironmentVariable($"{nameof(BotConfiguration)}__{nameof(BotConfiguration.BotToken)}"));
                    return new TelegramBotClient(options, httpClient);
                });

        services.AddOpenAIService(settings =>
        {
            settings.ApiKey = Environment.GetEnvironmentVariable($"{nameof(OpenAIConfiguration)}__{nameof(OpenAIConfiguration.OpenAIKey)}");
        });

        services.AddScoped<IBotBaseHandler, BotBaseHandler>();
        services.AddScoped<ICurrencyHandler, CurrencyHandler>();
        services.AddScoped<IQuestionHandler, QuestionHandler>();
        services.AddScoped<IImageHandler, ImageHandler>();

        services.AddControllers();
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "ArtyApp API" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            swagger.IncludeXmlComments(xmlPath);
        });
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}/swagger.json";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "ArtyApp API V1");
            c.RoutePrefix = "swagger";
        });

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", context =>
            {
                context.Response.Redirect("./swagger/index.html");
                return Task.FromResult(0);
            });
        });
    }
}
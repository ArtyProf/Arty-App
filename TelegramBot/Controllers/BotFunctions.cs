using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using TelegramBot.Interfaces;
using System.Threading;
using System;
using Telegram.Bot;
using TelegramBot.Configuration;
using Microsoft.Extensions.Options;

namespace TelegramBot.Controllers
{
    public class BotFunctions
    {
        private readonly IBotBaseHandler _botBaseHandler;
        private readonly ITelegramBotClient _botClient;
        private readonly BotConfiguration _botConfiguration;

        public BotFunctions(IBotBaseHandler botBaseHandler,
            ITelegramBotClient botClient,
            IOptions<BotConfiguration> botConfiguration)
        {
            _botBaseHandler = botBaseHandler;
            _botClient = botClient;
            _botConfiguration = botConfiguration.Value;
        }

        [FunctionName(nameof(Setup))]
        public async Task Setup([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            await _botClient.SetWebhookAsync($"{_botConfiguration.HostAddress}/api/{nameof(HandleUpdate)}");
        }

        [FunctionName(nameof(HandleUpdate))]
        public async Task HandleUpdate([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            CancellationToken cancellationToken)
        {
            var request = await req.ReadAsStringAsync();
            var update = JsonConvert.DeserializeObject<Update>(request);

            await _botBaseHandler.HandleUpdateAsync(update, cancellationToken);
        }
    }
}

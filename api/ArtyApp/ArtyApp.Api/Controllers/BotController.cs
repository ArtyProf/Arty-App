using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using ArtyApp.Interfaces;
using Telegram.Bot;
using ArtyApp.Configuration;
using Microsoft.Extensions.Options;

namespace ArtyApp.Controllers
{
    /// <summary>
    /// API for managing Telegram bot
    /// </summary>
    [Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        private readonly IBotBaseHandler _botBaseHandler;
        private readonly ITelegramBotClient _botClient;
        private readonly BotConfiguration _botConfiguration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="botBaseHandler"></param>
        /// <param name="botClient"></param>
        /// <param name="botConfiguration"></param>
        public BotController(IBotBaseHandler botBaseHandler,
            ITelegramBotClient botClient,
            IOptions<BotConfiguration> botConfiguration)
        {
            _botBaseHandler = botBaseHandler;
            _botClient = botClient;
            _botConfiguration = botConfiguration.Value;
        }

        /// <summary>
        /// Set up telegram bot
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost(nameof(Setup))]
        public async Task Setup(HttpRequest req)
        {
            await _botClient.SetWebhookAsync($"{_botConfiguration.HostAddress}/api/{nameof(HandleUpdate)}");
        }

        /// <summary>
        /// Handle telegram commands
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost(nameof(HandleUpdate))]
        public async Task HandleUpdate(HttpRequest req, CancellationToken cancellationToken)
        {
            var request = await new StreamReader(req.Body).ReadToEndAsync();
            var update = JsonConvert.DeserializeObject<Update>(request);

            await _botBaseHandler.HandleUpdateAsync(update, cancellationToken);
        }
    }
}
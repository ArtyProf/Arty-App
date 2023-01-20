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

namespace TelegramBot.Controllers
{
    public class BotFunction
    {
        private readonly IBotBaseHandler _botBaseHandler;

        public BotFunction(IBotBaseHandler botBaseHandler)
        {
            _botBaseHandler = botBaseHandler;
        }

        [FunctionName("BotFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation("HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Update update = JsonConvert.DeserializeObject<Update>(requestBody);

            await _botBaseHandler.HandleUpdateAsync(update, cancellationToken);

            return new OkResult();
        }
    }
}

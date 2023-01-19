using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBotfromArtyProf.Services;

namespace TelegramBotfromArtyProf.Controllers;

public class BotController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandlers handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}

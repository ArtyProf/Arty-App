using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBotfromArtyProf.Interfaces;

public interface ICurrencyHandler
{
    Task<Message> SendCurrencyExchange(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
}

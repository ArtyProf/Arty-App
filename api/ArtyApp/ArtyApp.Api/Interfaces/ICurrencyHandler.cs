using Telegram.Bot.Types;
using Telegram.Bot;
using System.Threading.Tasks;
using System.Threading;

namespace ArtyApp.Interfaces;

public interface ICurrencyHandler
{
    Task<Message> SendCurrencyExchange(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
}

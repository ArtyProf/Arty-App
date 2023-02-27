using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBot.Interfaces;

public interface IQuestionHandler
{
    Task<Message> AnswerTheQuestion(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
}

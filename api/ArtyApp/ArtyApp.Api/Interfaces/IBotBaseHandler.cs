using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot.Types;

namespace ArtyApp.Interfaces;

public interface IBotBaseHandler
{
    Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken);

    Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);
}

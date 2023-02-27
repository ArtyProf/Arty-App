using Microsoft.Extensions.Logging;
using OpenAI_API;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;

namespace TelegramBot.Services;

public class QuestionHandler : IQuestionHandler
{
    private readonly OpenAIAPI _openAIClient;
    private readonly ILogger<CurrencyHandler> _logger;

    public QuestionHandler(OpenAIAPI openAIClient,
        ILogger<CurrencyHandler> logger)
    {
        _openAIClient = openAIClient;
        _logger = logger;
    }

    public async Task<Message> AnswerTheQuestion(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Answer the question started.");

        var result = await _openAIClient.Completions.CreateCompletionAsync(message.Text, top_p: 1,model: "text-davinci-003", max_tokens: 256);
        return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: result.ToString(),
                cancellationToken: cancellationToken);
    }
}

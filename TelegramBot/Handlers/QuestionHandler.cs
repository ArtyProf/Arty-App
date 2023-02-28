using Microsoft.Extensions.Logging;
using OpenAI_API;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using TelegramBot.Configuration;
using Microsoft.Extensions.Options;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace TelegramBot.Handlers;

public class QuestionHandler : IQuestionHandler
{
    private readonly IOpenAIService _openAIClient;
    private readonly OpenAIConfiguration _openAIConfiguration;
    private readonly ILogger<QuestionHandler> _logger;

    public QuestionHandler(
        IOpenAIService openAIClient,
        IOptions<OpenAIConfiguration> openAIConfiguration,
        ILogger<QuestionHandler> logger)
    {
        _openAIClient = openAIClient;
        _openAIConfiguration = openAIConfiguration.Value;
        _logger = logger;
    }

    public async Task<Message> AnswerTheQuestion(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Answer the question started.");

        var result = await _openAIClient.Completions.CreateCompletion(new CompletionCreateRequest
            {
                MaxTokens = _openAIConfiguration.CompletionTokens,
                Temperature = _openAIConfiguration.CompletionTemperature,
                Model = _openAIConfiguration.CompletionModel
            }, cancellationToken: cancellationToken);

        if (result.Successful)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: result.ToString(),
                cancellationToken: cancellationToken);
        }

        return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Failed to create completion.",
                cancellationToken: cancellationToken);
    }
}

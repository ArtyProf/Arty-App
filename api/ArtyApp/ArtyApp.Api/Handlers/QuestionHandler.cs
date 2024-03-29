﻿using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using ArtyApp.Interfaces;
using ArtyApp.Configuration;
using Microsoft.Extensions.Options;
using OpenAI.GPT3.ObjectModels.RequestModels;
using static OpenAI.GPT3.ObjectModels.Models;
using OpenAI.GPT3.Interfaces;
using System.Linq;

namespace ArtyApp.Handlers;

public class QuestionHandler : IQuestionHandler
{
    private readonly IOpenAIService _openAIService;
    private readonly OpenAIConfiguration _openAIConfiguration;
    private readonly ILogger<QuestionHandler> _logger;

    public QuestionHandler(
        IOpenAIService openAIService,
        IOptions<OpenAIConfiguration> openAIConfiguration,
        ILogger<QuestionHandler> logger)
    {
        _openAIService = openAIService;
        _openAIConfiguration = openAIConfiguration.Value;
        _logger = logger;
    }

    public async Task<Message> AnswerTheQuestion(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Answer the question started.");
        
        var result = await _openAIService.Completions.CreateCompletion(new CompletionCreateRequest
            {
                Prompt = message.Text,
                Model = !string.IsNullOrWhiteSpace(_openAIConfiguration.CompletionModel) ? _openAIConfiguration.CompletionModel : TextDavinciV3,
                MaxTokens = int.TryParse(_openAIConfiguration.CompletionTokens, out var maxTokens) ? maxTokens : 2048,
                Temperature = float.TryParse(_openAIConfiguration.CompletionTemperature, out var temperature) ? temperature : 0.8f,
                N = 1
            },
            cancellationToken: cancellationToken);

        if (result.Successful && result.Choices.Any())
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: result.Choices.First().Text,
                cancellationToken: cancellationToken);
        }

        return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Failed to create completion.",
                cancellationToken: cancellationToken);
    }
}

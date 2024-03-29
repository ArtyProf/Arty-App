﻿using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using ArtyApp.Interfaces;
using Telegram.Bot.Types.InlineQueryResults;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArtyApp.Handlers;

public class BotBaseHandler : IBotBaseHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<BotBaseHandler> _logger;
    private readonly ICurrencyHandler _currencyHandler;
    private readonly IQuestionHandler _questionHandler;
    private readonly IImageHandler _imageHandler;

    public BotBaseHandler(ITelegramBotClient botClient,
        ILogger<BotBaseHandler> logger,
        ICurrencyHandler currencyHandler,
        IQuestionHandler questionHandler,
        IImageHandler imageHandler)
    {
        _botClient = botClient;
        _logger = logger;
        _currencyHandler = currencyHandler;
        _questionHandler = questionHandler;
        _imageHandler = imageHandler;
    }

    public Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            { EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            { InlineQuery: { } inlineQuery } => BotOnInlineQueryReceived(inlineQuery, cancellationToken),
            { ChosenInlineResult: { } chosenInlineResult } => BotOnChosenInlineResultReceived(chosenInlineResult, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        if (message.Text is not { } messageText)
            return;

        if (messageText.Contains('@'))
        {
            messageText = messageText.Replace("@Arty_ProfBot", "");
        }

        var command = messageText.Split(' ');

        Message sentMessage;
        if (command.Length < 2)
        {
            sentMessage = await Usage(_botClient, message, cancellationToken);
        }
        else
        {
            var action = command[0] switch
            {
                "/start" => SendGreetings(_botClient, message, cancellationToken),
                "/currency" => _currencyHandler.SendCurrencyExchange(_botClient, message, cancellationToken),
                "/question" => _questionHandler.AnswerTheQuestion(_botClient, message, cancellationToken),
                "/image" => _imageHandler.GetImage(_botClient, message, cancellationToken),
                _ => Usage(_botClient, message, cancellationToken)
            };
            sentMessage = await action;
        }
        
        _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);

        static async Task<Message> SendGreetings(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Greeting! You do your first step!",
                cancellationToken: cancellationToken);
        }

        static async Task<Message> Usage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            const string usage = "Usage:\r\n" +
                                 "/start - Greeting\r\n" +
                                 "/currency - Currency Exchange rate.\nExample: /currency UAH USD 10\r\n" +
                                 "/question - Ask any question. Based on Open AI (ChatGPT).\nExample: /question Top movie titles 2023\r\n" +
                                 "/image - Image description. Based on Open AI (ChatGPT).\nExample: /image orange sky";

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }

    // Process Inline Keyboard callback data
    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

        await _botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Received {callbackQuery.Data}",
            cancellationToken: cancellationToken);

        await _botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: $"Received {callbackQuery.Data}",
            cancellationToken: cancellationToken);
    }

    private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received inline query from: {InlineQueryFromId}", inlineQuery.From.Id);

        InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "1",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent("hello"))
        };

        await _botClient.AnswerInlineQueryAsync(
            inlineQueryId: inlineQuery.Id,
            results: results,
            cacheTime: 0,
            isPersonal: true,
            cancellationToken: cancellationToken);
    }

    private async Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received inline result: {ChosenInlineResultId}", chosenInlineResult.ResultId);

        await _botClient.SendTextMessageAsync(
            chatId: chosenInlineResult.From.Id,
            text: $"You chose result with Id: {chosenInlineResult.ResultId}",
            cancellationToken: cancellationToken);
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }
}
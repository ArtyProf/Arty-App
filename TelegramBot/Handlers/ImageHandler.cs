using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using System.Linq;
using OpenAI.GPT3.Interfaces;
using System.IO;
using System;

namespace TelegramBot.Handlers;

public class ImageHandler : IImageHandler
{
    private readonly IOpenAIService _openAIService;
    private readonly ILogger<ImageHandler> _logger;

	public ImageHandler(IOpenAIService openAIService,
		ILogger<ImageHandler> logger)
	{
        _openAIService = openAIService;
		_logger = logger;
	}

	public async Task<Message> GetImage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Image generation started.");

		var result = await _openAIService.Image.CreateImage(
			new ImageCreateRequest
			{
                Prompt = message.Text,
				N = 1,
				Size = StaticValues.ImageStatics.Size.Size1024,
				ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Base64
            }, cancellationToken);

		if (result.Successful)
		{
            var imageStream = new MemoryStream(Convert.FromBase64String(result.Results.First().B64));
            return await botClient.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: new InputMedia(imageStream, "image.jpg"),
                cancellationToken: cancellationToken);
        }

		return await botClient.SendTextMessageAsync(
				chatId: message.Chat.Id,
				text: "Failed to create image.",
                cancellationToken: cancellationToken);
	}
}
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using System.Linq;

namespace TelegramBot.Handlers;

public class ImageHandler : IImageHandler
{
    private readonly IOpenAIService _openAIClient;
    private readonly ILogger<ImageHandler> _logger;

	public ImageHandler(IOpenAIService openAIClient,
		ILogger<ImageHandler> logger)
	{
		_openAIClient = openAIClient;
		_logger = logger;
	}

	public async Task<Message> GetImage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Image generation started.");

		var result = await _openAIClient.Image.CreateImage(
			new ImageCreateRequest
			{
                Prompt = message.Text,
				N = 1,
				Size = StaticValues.ImageStatics.Size.Size1024,
				ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Base64
            });

		if (result.Successful)
		{
            return await botClient.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: result.Results.First().B64,
                cancellationToken: cancellationToken);
        }

		return await botClient.SendTextMessageAsync(
				chatId: message.Chat.Id,
				text: "Failed to create image.",
                cancellationToken: cancellationToken);
	}
}
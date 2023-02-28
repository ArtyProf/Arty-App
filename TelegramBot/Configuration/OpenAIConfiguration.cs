namespace TelegramBot.Configuration;

public class OpenAIConfiguration
{
    public string OpenAIKey { get; set; }

    public string CompletionModel { get; set; }

    public float CompletionTemperature { get; set; }

    public int CompletionTokens { get; set; }
}

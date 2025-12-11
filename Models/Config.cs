namespace Doan.Models
{
    public class Config
    {
        public string Provider { get; set; } = "OpenAI";
        public OpenAIConfig OpenAI { set; get; } = new();
    }
    public class OpenAIConfig
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ChatModel { get; set; } = "gpt-4o-mini";
    }
}

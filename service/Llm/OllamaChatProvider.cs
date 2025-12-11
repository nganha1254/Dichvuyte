namespace Doan.service.Llm
{
    public class OllamaChatProvider : ILlmChatProvider
    {
        public async Task<string> AskAsync(string system, string user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

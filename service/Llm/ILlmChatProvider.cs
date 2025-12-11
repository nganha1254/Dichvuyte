namespace Doan.service.Llm
{
    public interface ILlmChatProvider
    {
        Task<string> AskAsync(string system, string user, CancellationToken cancellationToken = default);
    }
}

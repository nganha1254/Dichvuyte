using Doan.service.Llm;

namespace Doan.service
{
    public class RagPipeline
    {
        private readonly ILlmChatProvider _llmChatProvider;
        public RagPipeline(ILlmChatProvider llmChatProvider)
        {
            _llmChatProvider = llmChatProvider;
        }

        public async Task<string> AskAsync(string question, CancellationToken cancellationToken = default)
        {
            string relevantDocs = string.Empty;
            var systemPrompt = "You are a helpful assistant that provides concise and accurate answers based on the provided context.";
            var userPrompt = $"{string.Join("\n\n", relevantDocs)}\n\nQuestion: {question}";
            var answer = await _llmChatProvider.AskAsync(systemPrompt, userPrompt, cancellationToken);

            return answer;
        }
    }

}

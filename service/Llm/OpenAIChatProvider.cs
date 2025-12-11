using OpenAI.Chat;

namespace Doan.service.Llm
{
    public class OpenAIChatProvider : ILlmChatProvider
    {
        private readonly ChatClient _chatClient;

        public OpenAIChatProvider(ChatClient chatClient)
        {
            _chatClient = chatClient;
        }

        public async Task<string> AskAsync(string system, string user, CancellationToken cancellationToken = default)
        {
            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(system),
                new UserChatMessage(user)
            };

            // SDK cũ -> KHÔNG có tham số model
            var completion = await _chatClient.CompleteChatAsync(
                messages: messages,
                cancellationToken: cancellationToken
            );

            return completion.Value.Content.Count > 0
                ? completion.Value.Content[0].Text
                : string.Empty;
        }
    }
}

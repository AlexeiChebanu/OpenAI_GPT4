using OpenAI.Models;
using OpenAI_Chebanu.Chat;

namespace OpenAI.Chat
{
     interface IChatEndpoint
    {
        Task<ChatResult> CreateChatCompletionAsync(ChatRequest request);
        Task<ChatResult> CreateChatCompletionAsync(ChatRequest request, int numOutputs = 5);
        Task<ChatResult> CreateChatCompletionAsync(IList<ChatMessage> messages, Model model = null, double? temperature = null, double? top_p = null, int? numOutputs = null, int? max_tokens = null, double? frequencyPenalty = null, double? presencePenalty = null, IReadOnlyDictionary<string, float> logitBias = null, params string[] stopSequences);
        Task<ChatResult> CreateChatCompletionAsync(params ChatMessage[] messages);

    }
}
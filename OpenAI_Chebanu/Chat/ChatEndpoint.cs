using OpenAI.Models;
using OpenAI_Chebanu.Chat;

namespace OpenAI.Chat
{
    public class ChatEndpoint : EndpointBase, IChatEndpoint
    {
        public ChatRequest DefChatRequestArgs { get; set; } = new ChatRequest() { Model = Models.Model.ChatGPTTurbo0301 };

        protected override string Endpoint { get { return "chat/completions"; } }
        public ChatEndpoint (OpenAIClient api) : base(api) { }

        public Conversation CreateConversation()
        {
            return new Conversation(this, defaultChatRequestArgs: DefChatRequestArgs);
        }

        public async Task<ChatResult> CreateChatCompletionAsync(ChatRequest request)
        {
            return await HttpPost<ChatResult>(postData: request);
        }

        public Task<ChatResult> CreateChatCompletionAsync(ChatRequest request, int numOutputs = 5)
        {
            request.NumChoicesPerMessage = numOutputs;
            return CreateChatCompletionAsync(request);
        }

        public Task<ChatResult> CreateChatCompletionAsync(IList<ChatMessage> messages, Model model = null, double? temperature = null, double? top_p = null, int? numOutputs = null, int? max_tokens = null, double? frequencyPenalty = null, double? presencePenalty = null, IReadOnlyDictionary<string, float> logitBias = null, params string[] stopSequences)
        {
            ChatRequest request = new ChatRequest(DefChatRequestArgs)
            {
                Messages = messages,
                Model = model ?? DefChatRequestArgs.Model,
                Temperature = temperature ?? DefChatRequestArgs.Temperature,
                TopP = top_p ?? DefChatRequestArgs.TopP,
                NumChoicesPerMessage = numOutputs ?? DefChatRequestArgs.NumChoicesPerMessage,
                MultipleStopSequences = stopSequences ?? DefChatRequestArgs.MultipleStopSequences,
                MaxTokens = max_tokens ?? DefChatRequestArgs.MaxTokens,
                FrequencyPenalty = frequencyPenalty ?? DefChatRequestArgs.FrequencyPenalty,
                PresencePenalty = presencePenalty ?? DefChatRequestArgs.PresencePenalty,
                LogitBias = logitBias ?? DefChatRequestArgs.LogitBias
            };
            return CreateChatCompletionAsync(request);
        }

        public Task<ChatResult> CreateChatCompletionAsync(params ChatMessage[] messages)
        {
            ChatRequest request = new ChatRequest(DefChatRequestArgs)
            {
                Messages = messages
            };
            return CreateChatCompletionAsync(request);
        }
    }
}
using OpenAI.Models;
using OpenAI_Client.Completion;

namespace OpenAI.Completion
{
    public class CompletionEndpoint : EndpointBase, ICompletionEndpoint
    { 

        public CompletionRequest DefaultCompletionRequestArgs { get; set; } = new CompletionRequest() { Model = Model.DavinciText};

        protected override string Endpoint { get { return "completions"; } }

        internal CompletionEndpoint(OpenAIClient api): base(api) { }

        public async Task<CompletionResult> CreateCompletionAsync(CompletionRequest request) => 
            await HttpPost<CompletionResult>(postData: request);

        public Task<CompletionResult> CreateCompletionsAsync(CompletionRequest request, int numOut=3)
        {
            request.NumChoicesPerPrompt= numOut;
            return CreateCompletionAsync(request);
        }

        public Task<CompletionResult> CreateCompletionAsync(string prompt,
            Model model = null,
            int? max_tokens = null,
            double? temperature = null,
            double? top_p = null,
            int? numOutputs = null,
            double? presencePenalty = null,
            double? frequencyPenalty = null,
            int? logProbs = null,
            bool? echo = null,
            params string[] stopSequences
            )
        {
            CompletionRequest request = new CompletionRequest(DefaultCompletionRequestArgs)
            {
                Prompt = prompt,
                Model = model ?? DefaultCompletionRequestArgs.Model,
                MaxTokens = max_tokens ?? DefaultCompletionRequestArgs.MaxTokens,
                Temperature = temperature ?? DefaultCompletionRequestArgs.Temperature,
                TopP = top_p ?? DefaultCompletionRequestArgs.TopP,
                NumChoicesPerPrompt = numOutputs ?? DefaultCompletionRequestArgs.NumChoicesPerPrompt,
                PresencePenalty = presencePenalty ?? DefaultCompletionRequestArgs.PresencePenalty,
                FrequencyPenalty = frequencyPenalty ?? DefaultCompletionRequestArgs.FrequencyPenalty,
                Logprobs = logProbs ?? DefaultCompletionRequestArgs.Logprobs,
                Echo = echo ?? DefaultCompletionRequestArgs.Echo,
                MultipleStopSequences = stopSequences ?? DefaultCompletionRequestArgs.MultipleStopSequences
            };
            return CreateCompletionAsync(request);
        }


        public Task<CompletionResult> CreateCompletionAsync(params string[] prompts)
        {
            CompletionRequest request = new CompletionRequest(DefaultCompletionRequestArgs)
            {
                MultiplePrompts = prompts
            };
            return CreateCompletionAsync(request);
        }



        public IAsyncEnumerable<CompletionResult> StreamCompletionEnumerableAsync(CompletionRequest request)
        {
            request = new CompletionRequest(request) { Stream = true };
            return HttpStreamingRequest<CompletionResult>(Url, HttpMethod.Post, request);
        }

        public async Task StreamCompletionAsync(CompletionRequest request, Action<int, CompletionResult> resultHandler)
        {
            int index = 0;
            await foreach (var res in StreamCompletionEnumerableAsync(request))
            {
                resultHandler(index++, res);
            }
        }

        public async Task StreamCompletionAsync(CompletionRequest request, Action<CompletionResult> resultHandler)
        {
            await foreach (var res in StreamCompletionEnumerableAsync(request))
            {
                resultHandler(res);
            }
        }

        public IAsyncEnumerable<CompletionResult> StreamCompletionEnumerableAsync(string prompt,
            Model model = null,
            int? max_tokens = null,
            double? temperature= null,
            double? top_p = null,
            int? numOutputs = null,
            double? presencePenalty = null,
            double? frequencyPenalty = null,
            int? logProbs = null,
            bool? echo = null,
            params string[] stopSequences)
        {
            CompletionRequest request = new CompletionRequest(DefaultCompletionRequestArgs)
            {
                Prompt = prompt,
                Model = model ?? DefaultCompletionRequestArgs.Model,
                MaxTokens = max_tokens ?? DefaultCompletionRequestArgs.MaxTokens,
                Temperature = temperature ?? DefaultCompletionRequestArgs.Temperature,
                TopP = top_p ?? DefaultCompletionRequestArgs.TopP,
                NumChoicesPerPrompt = numOutputs ?? DefaultCompletionRequestArgs.NumChoicesPerPrompt,
                PresencePenalty = presencePenalty?? DefaultCompletionRequestArgs.PresencePenalty,
                FrequencyPenalty = frequencyPenalty ?? DefaultCompletionRequestArgs.FrequencyPenalty,
                Logprobs = logProbs?? DefaultCompletionRequestArgs.Logprobs,
                Echo = echo ?? DefaultCompletionRequestArgs.Echo,
                MultipleStopSequences = stopSequences ?? DefaultCompletionRequestArgs.MultipleStopSequences,
                Stream =true
            };

            return StreamCompletionEnumerableAsync(request);
        }

        public async Task<string> CreateAndFormatCompletion(CompletionRequest request)
        {
            string prompt = request.Prompt;
            var result = await CreateCompletionAsync(request);
            return prompt + result.ToString();
        }

        public async Task<string> GetCompletion(string prompt)
        {
            CompletionRequest request = new CompletionRequest(DefaultCompletionRequestArgs)
            {
                Prompt = prompt,
                NumChoicesPerPrompt = 1
            };
            var result = await CreateCompletionAsync();
            return result.ToString();
        }
    }
}
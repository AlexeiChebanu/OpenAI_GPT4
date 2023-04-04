using OpenAI.Models;
using OpenAI_Client.Embedding;

namespace OpenAI.Embedding
{
    public class EmbeddingEndpoint : EndpointBase, IEmbeddingEndpoint
    {
        public EmbeddingRequest DefaultEmbeddingRequestArgs { get; set; } = new EmbeddingRequest() { Model = Model.AdaTextEmbedding };

        protected override string Endpoint { get { return "embeddings"; } }

        public EmbeddingEndpoint(OpenAIClient api) : base(api) { }
        public async Task<EmbeddingResult> CreateEmbeddingAsync(string input)
        {
            EmbeddingRequest req = new EmbeddingRequest(DefaultEmbeddingRequestArgs.Model, input); 
            return await CreateEmbeddingAsync(req);
        }

        public async Task<EmbeddingResult> CreateEmbeddingAsync(EmbeddingRequest request)
            => await HttpPost<EmbeddingResult>(postData: request);

        public async Task<float[]> GetEmbeddingAsync(string input)
        {
            EmbeddingRequest request = new EmbeddingRequest(DefaultEmbeddingRequestArgs.Model, input);
            EmbeddingResult result = await CreateEmbeddingAsync(request);
            return result?.Data?[0]?.Embedding;
        }
    }
}
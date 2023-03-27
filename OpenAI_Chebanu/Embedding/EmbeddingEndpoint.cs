namespace OpenAI.Embedding
{
    public class EmbeddingEndpoint
    {
        private OpenAIClient openAIClient;

        public EmbeddingEndpoint(OpenAIClient openAIClient)
        {
            this.openAIClient = openAIClient;
        }
    }
}
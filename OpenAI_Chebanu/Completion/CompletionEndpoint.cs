namespace OpenAI.Completion
{
    public class CompletionEndpoint
    {
        private OpenAIClient openAIClient;

        public CompletionEndpoint(OpenAIClient openAIClient)
        {
            this.openAIClient = openAIClient;
        }
    }
}
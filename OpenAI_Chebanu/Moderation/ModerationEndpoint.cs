namespace OpenAI.Moderation
{
    public class ModerationEndpoint
    {
        private OpenAIClient openAIClient;

        public ModerationEndpoint(OpenAIClient openAIClient)
        {
            this.openAIClient = openAIClient;
        }
    }
}
namespace OpenAI.Image
{
    public class ImageGenerationEndpoint
    {
        private OpenAIClient openAIClient;

        public ImageGenerationEndpoint(OpenAIClient openAIClient)
        {
            this.openAIClient = openAIClient;
        }
    }
}
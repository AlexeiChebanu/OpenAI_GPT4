namespace OpenAI.Files
{
    public class FilesEndpoint
    {
        private OpenAIClient openAIClient;

        public FilesEndpoint(OpenAIClient openAIClient)
        {
            this.openAIClient = openAIClient;
        }
    }
}
using OpenAI_Client.Image;

namespace OpenAI.Image
{
    public class ImageGenerationEndpoint : EndpointBase, IImageGenerationEndpoint
    {
        protected override string Endpoint { get { return "images/generations"; } }

        internal ImageGenerationEndpoint(OpenAIClient openAIClient) : base(openAIClient) { }


        public async Task<ImageResult> CreateImageAsync(string input)
        {
            ImageRequest req = new ImageRequest(prompt: input);
            return await CreateImageAsync(req);
        }

        public async Task<ImageResult> CreateImageAsync(ImageRequest request)
        {
            return await HttpPost<ImageResult>(postData: request);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_Client.Image
{
    interface IImageGenerationEndpoint
    {
        Task<ImageResult> CreateImageAsync(ImageRequest request);

        Task<ImageResult> CreateImageAsync(string input);
    }
}

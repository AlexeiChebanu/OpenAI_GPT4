using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_Client.Embedding
{
     interface IEmbeddingEndpoint
    {
        EmbeddingRequest DefaultEmbeddingRequestArgs { get; set; }

        Task<EmbeddingResult> CreateEmbeddingAsync(string input);

        Task<EmbeddingResult> CreateEmbeddingAsync(EmbeddingRequest request);

        Task<float[]> GetEmbeddingAsync(string input);

    }
}

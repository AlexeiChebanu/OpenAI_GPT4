using Newtonsoft.Json;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_Client.Embedding
{
    public class EmbeddingResult : ApiResultBase
    {

        [JsonProperty("data")]
        public List<Data> Data { get; set; }

        [JsonProperty("usage")]
        public Usage Usage { get; set; }

        public static implicit operator float[](EmbeddingResult embeddingResult)
            => embeddingResult.Data.FirstOrDefault()?.Embedding;

    }

    public class Data
    {

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("embedding")]
        public float[] Embedding { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

    }
}

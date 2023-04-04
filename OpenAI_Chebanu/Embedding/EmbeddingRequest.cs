using Newtonsoft.Json;
using OpenAI;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_Client.Embedding
{
    public class EmbeddingRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("input")]
        public string Input { get; set; }

        public EmbeddingRequest() { }

        public EmbeddingRequest(Model model, string input)
        {
            Model = model;
            Input = input;
        }

        public EmbeddingRequest(string input)
        {
            Model = OpenAI.Models.Model.AdaTextEmbedding;
            Input = input;
        }

    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_Client.Image
{
    public class ImageRequest
    {
        /// <summary>
		/// A text description of the desired image(s). The maximum length is 1000 characters.
		/// </summary>
		[JsonProperty("prompt")]
        public string Prompt { get; set; }

        /// <summary>
        /// How many different choices to request for each prompt.  Defaults to 1.
        /// </summary>
        [JsonProperty("n")]
        public int? NumOfImages { get; set; } = 1;

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse. Optional.
        /// </summary>
        [JsonProperty("user")]
        public string User { get; set; }

        /// <summary>
        /// The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024. Defauls to 1024x1024
        /// </summary>
        [JsonProperty("size"), JsonConverter(typeof(ImageSize.ImageSizeJsonConverter))]
        public ImageSize Size { get; set; }

        /// <summary>
        /// The format in which the generated images are returned. Must be one of url or b64_json. Defaults to Url.
        /// </summary>
        [JsonProperty("response_format"), JsonConverter(typeof(ImageResponse.ImageResponseJsonConverter))]
        public ImageResponse Response { get; set; }

        public ImageRequest()
        {

        }
        public ImageRequest(
            string prompt,
            int? numOfImages = 1,
            ImageSize size = null,
            string user = null,
            ImageResponse responseFormat = null)
        {
            this.Prompt = prompt;
            this.NumOfImages = numOfImages;
            this.User = user;
            this.Size = size ?? ImageSize.size1024;
            this.Response = responseFormat ?? ImageResponse.Url;
        }
    }
}

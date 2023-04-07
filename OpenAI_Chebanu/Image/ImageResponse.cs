using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_Client.Image
{
    public class ImageResponse
    {
        private ImageResponse(string value)
        {
            Value = value;
        }

        private string Value { get; set; }

        public static ImageResponse Url { get { return new ImageResponse("url"); } }

        public static ImageResponse B64_json { get { return new ImageResponse("b64_json"); } }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator String(ImageResponse value) { return value; }

        internal class ImageResponseJsonConverter : JsonConverter<ImageResponse>
        {
            public override ImageResponse? ReadJson(JsonReader reader, Type objectType, ImageResponse? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new ImageResponse(reader.ReadAsString());
            }

            public override void WriteJson(JsonWriter writer, ImageResponse? value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }


    }
}

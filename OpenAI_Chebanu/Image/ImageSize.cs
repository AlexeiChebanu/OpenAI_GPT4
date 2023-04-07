using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_Client.Image
{
    public class ImageSize
    {
        private ImageSize(string val) { Value = val; }

        
        public string Value { get; set; }

        public static ImageSize size256 { get { return new ImageSize("256x256"); } }

        public static ImageSize size512 { get { return new ImageSize("512x512"); } }

        public static ImageSize size1024 { get { return new ImageSize("1024x1024"); } }


        public override string ToString()
        {
            return Value;
        }

        public static implicit operator String(ImageSize size) { return size; }

        internal class ImageSizeJsonConverter : JsonConverter<ImageSize>
        {
            public override void WriteJson(JsonWriter writer, ImageSize? value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }

            public override ImageSize? ReadJson(JsonReader reader, Type objectType, ImageSize? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new ImageSize(reader.ReadAsString());
            }

        }

    }
}

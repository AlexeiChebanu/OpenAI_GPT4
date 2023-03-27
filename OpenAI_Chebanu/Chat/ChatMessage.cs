using Newtonsoft.Json;

namespace OpenAI_Chebanu.Chat
{
    public class ChatMessage
    {
        public ChatMessage()
        {
            this.Role = ChatMessageRole.User;
        }

        public ChatMessage(ChatMessageRole role, string content)
        {
            this.Role = role;
            this.Content = content;
        }

        [JsonProperty("role")]
        internal string rawRole { get; set; }

        [JsonIgnore]
        public ChatMessageRole Role
        {
            get
            {
                return ChatMessageRole.FromString(rawRole);
            }
            set
            {
                rawRole = value.ToString();
            }
        }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}

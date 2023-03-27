namespace OpenAI_Chebanu.Chat
{
    public class ChatMessageRole : IEquatable<ChatMessageRole>
    {
        private ChatMessageRole(string value) { Value = value; }


        public static ChatMessageRole FromString(string roleName)
        {
            switch (roleName)
            {
                case "system":
                    return ChatMessageRole.System;
                case "user":
                    return ChatMessageRole.User;
                case "assistant":
                    return ChatMessageRole.Assistant;
                default:
                    return null;
            }
        }

        private string Value { get; }


        public static ChatMessageRole System { get; } = new ChatMessageRole("system");
        public static ChatMessageRole User { get; } = new ChatMessageRole("user");
        public static ChatMessageRole Assistant { get; } = new ChatMessageRole("assistant");

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            return Value.Equals((obj as ChatMessageRole).Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(ChatMessageRole other)
        {
            return Value.Equals(other.Value);
        }


        public static implicit operator String(ChatMessageRole value) { return value; }

    }
}
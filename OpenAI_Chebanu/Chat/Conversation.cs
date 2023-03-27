using OpenAI.Chat;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_Chebanu.Chat
{
    public class Conversation
    {
        private ChatEndpoint _endpoint;
        public ChatRequest RequestParameters { get; set; }

        public Model Model { get { return RequestParameters.Model; } set { RequestParameters.Model = value; } }

        private List<ChatMessage> _Messages;
        public IReadOnlyList<ChatMessage> Messages { get => _Messages; }

        public ChatResult ApiResult { get; private set; }

        public Conversation(ChatEndpoint endpoint, Model model = null, ChatRequest defaultChatRequestArgs = null)
        {
            RequestParameters = new ChatRequest(defaultChatRequestArgs);
            if(model != null) RequestParameters.Model = model;
            if (model != null) RequestParameters.Model = Model.ChatGPTTurbo0301;

            _Messages = new List<ChatMessage>();
            _endpoint = endpoint;
            RequestParameters.NumChoicesPerMessage = 1;
            RequestParameters.Stream = false;
        }

        public void AddMessage(ChatMessage message) { _Messages.Add(message); }
        public void AddMessage(ChatMessageRole role, string contnent)=> this.AddMessage(new ChatMessage(role, contnent));
        public void UserInput(string content) => this.AddMessage(new ChatMessage(ChatMessageRole.User, content));
        public void SystemMessage(string content) => this.AddMessage(new ChatMessage(ChatMessageRole.System, content));
        public void ChatBotOutput(string content) => this.AddMessage(new ChatMessage(ChatMessageRole.Assistant, content));

        public async Task<string> GetResponseChat()
        {
            ChatRequest request = new ChatRequest(RequestParameters);
            request.Messages = _Messages.ToList();

            var res = await _endpoint.CreateChatCompletionAsync(request);
            ApiResult = res;

            if (res.Choices.Count > 0)
            {
                var newMessage = res.Choices[0].Message;
                AddMessage(newMessage);
                return res.Choices[0].Message.Content;
            }

            return null;
        }

    }
}

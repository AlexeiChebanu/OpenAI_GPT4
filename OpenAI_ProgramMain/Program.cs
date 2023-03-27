using OpenAI;

OpenAIClient api = new OpenAIClient("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"); // shorthand

var chat = api.Chat.CreateConversation();

/// give instruction as System
chat.AppendSystemMessage("Give the whole definition ");

while (true)
{

    // give a few examples as user and assistant
    string req = Console.ReadLine().ToString();

    if (string.IsNullOrEmpty(req))
    {
        return 0;
    }

    // now let's ask it a question'
    chat.AppendUserInput(req);
    // and get the response
    string response = await chat.GetResponseChat();
    Console.WriteLine(response);
}

// the entire chat history is available in chat.Messages
/*foreach (ChatMessage msg in chat.Messages)
{
    Console.WriteLine($"{msg.Role}: {msg.Content}");
}*/
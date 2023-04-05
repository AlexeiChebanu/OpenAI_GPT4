using OpenAI;
using OpenAI.Completion;
using OpenAI.Models;
using OpenAI_Chebanu.Chat;
using OpenAI_Client.Completion;
using OpenAI_Client.Embedding;

OpenAIClient api = new OpenAIClient("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");

var response = await api.Files.GetFilesAsync();
foreach (var file in response)
{
    Console.WriteLine(file.Name);
}
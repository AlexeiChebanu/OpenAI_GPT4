using OpenAI;
using OpenAI.Completion;
using OpenAI.Models;
using OpenAI_Chebanu.Chat;
using OpenAI_Client.Completion;
using OpenAI_Client.Embedding;
using OpenAI_Client.Image;

OpenAIClient api = new OpenAIClient("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");

var result = await api.ImageGenerations.CreateImageAsync(new ImageRequest("A drawing of a computer writing a test", 1, ImageSize.size512));

Console.WriteLine(result.Data[0].Url);
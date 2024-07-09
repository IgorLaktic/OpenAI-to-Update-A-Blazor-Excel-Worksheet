using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Chat;
using OpenAI.Models;

using System.ClientModel;

namespace Api
{ 
    public class OpenAIService 
    {
        private readonly OpenAIServiceOptions _openAIServiceOptions;

        public OpenAIService(
            IOptions<OpenAIServiceOptions> openAIServiceOptions)
        {
            _openAIServiceOptions = openAIServiceOptions.Value;
        }

        // OpenAI
       
        public async Task<List<ChatMessage>> CallOpenAIService(List<ChatMessage> messages)
        {
            var ApiKey = _openAIServiceOptions.ApiKey;
            ChatClient client = new ChatClient("gpt-4o", ApiKey);
            ClientResult<ChatCompletion> result = await client.CompleteChatAsync(messages);

            messages.Add(new AssistantChatMessage(result.Value));
            return messages;
        }

      
        public async Task<string> CallOpenAIServiceToGetJSON(string? jsonText)
        {
            // Get the API key from the appsettings.json file
            var ApiKey = _openAIServiceOptions.ApiKey;
            ChatClient client = new ChatClient("gpt-4o",  ApiKey);

            List<ChatMessage> messages = new List<ChatMessage>()
            {
                new SystemChatMessage( $"Please correct this json to make it valid. Return only the valid json in a collection called data: {jsonText}")
            };
            ChatCompletionOptions options = new ChatCompletionOptions()
            {
                ResponseFormat = ChatResponseFormat.JsonObject
            };
            ClientResult<ChatCompletion> result = await client.CompleteChatAsync(messages, options);
            return result.Value.Content[0].Text;
        }
    }
}
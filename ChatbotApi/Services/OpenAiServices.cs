using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatbotApi.Services
{
    public class OpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly GetKnbResponse _knbReader; // 💡 Dependency Injection für das Datei-Lesen

        public OpenAiService(HttpClient httpClient, GetKnbResponse knbReader)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _knbReader = knbReader;
        }

        public async Task<string> GetChatResponse(string userInput)
        {
            string systemMessage = await _knbReader.ReadFileAsync();
            if (string.IsNullOrWhiteSpace(systemMessage))
            {
                systemMessage = "Du bist ein hilfreicher Assistent für alle moralischen und ethischen Fragen.";
            }

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = systemMessage }, // ✅ Dateiinhalt als System-Message
                    new { role = "user", content = userInput }
                },
                max_tokens = 200
            };

            string jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            if (!response.IsSuccessStatusCode)
            {
                return $"Fehler: {response.StatusCode}";
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseContent);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }
    }
}

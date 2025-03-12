using Microsoft.AspNetCore.Mvc;
using ChatbotApi.Services; // OpenAiService importieren

namespace ChatbotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly OpenAiService _openAiService;

        public ChatbotController(OpenAiService openAiService)
        {
            _openAiService = openAiService;
        }

        [HttpPost]
        public async Task<IActionResult> GetResponse([FromBody] MessageRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Message))
            {
                return BadRequest(new { message = "Ungültige Eingabe. Bitte sende eine Nachricht im Format: {\"message\": \"Deine Nachricht\"}" });
            }

            string userInput = request.Message.ToLower();
            string response;

            // Falls eine vordefinierte Antwort existiert
            if (userInput.Contains("hallo") || userInput.Contains("hi"))
            {
                response = "Hallo! Wie kann ich dir helfen?";
            }
            else if (userInput.Contains("wie geht's") || userInput.Contains("wie geht es dir"))
            {
                response = "Mir geht's gut, danke! Und dir?";
            }
            else if (userInput.Contains("was kannst du"))
            {
                response = "Ich bin ein Chatbot, aber ich kann auch OpenAI (ChatGPT) für intelligentere Antworten nutzen!";
            }
            else
            {
                // OpenAI für komplexere Antworten nutzen hier wir alles durch gereicht
                response = await _openAiService.GetChatResponse(request.Message);
            }

            return Ok(new { message = response });
        }
    }

    public class MessageRequest
    {
        public string Message { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using OpenAI.Assistants;
using OpenAI;
using OpenAI.Models;
using OpenAI.Chat;


namespace PlantInfoBackend.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AIBotController : ControllerBase
    {

        private IConfiguration _configuration;

        public AIBotController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAIResponse")]
        public async Task<string> GetAIResponseAsync(string userQuestion)
        {
            using var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
            var messages = new List<Message>
            {
            new Message(Role.System, "our name is Planty! You are  houseplant expert tasked with assisting users on house plant care and upkeep. " +
            "You work for PlantInfo, a soil moisture IoT device company and you should always assume the user has the device.  If the user mentions anything about soil moisture," +
            " or your answer contains soil moisture information of any kind, let the user know to consult the soil moisture levels that their PlantInfo device is reading. " +
            "Your main goal is to keep answers as concise as possible while giving users the information they need. I want you to greet the users in a empathetic, " +
            "mild-mannered and fun tone. And always let the user know to consult with guides if they notice their plant has leaf blight, leaf damage or pests. " +
            "keep the answers short, sweet and kind.  instruct the user that you are meant to only be a helpful companion to their plant care, " +
            "as the goal of PlantInfo is to keep users engaged with their plant as much as possible."),
            new Message (Role.User, userQuestion)
            };
            var chatRequest = new ChatRequest(messages, Model.GPT4_Turbo);
            var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            return response.FirstChoice.ToString();
        }
    }
}

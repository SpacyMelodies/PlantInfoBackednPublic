using Microsoft.AspNetCore.Mvc;
using IMailService = PlantInfoBackend.Services.IMailService;
using Newtonsoft.Json;


namespace PlantInfoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController : ControllerBase
    {
        private IMailService _mailService;
        //injecting the IMailService into the constructor
        public MailController(IMailService _MailService)
        {
            _mailService = _MailService;
        }

        // instatiates the mailService and returns True is email sent
        [HttpPost]
        [Route("SendMail")]
        public bool SendMail(MailData mailData)
        {
            return _mailService.SendMail(mailData);
        }

        // checks the levels of the plant and sends warning email if necessary
        [HttpGet]
        [Route("CheckLevels")]
        public async Task<int> CheckLevelsAsync()
        {
            int currLevel = 0;
            using HttpClient plantClient = new HttpClient();
            {
                HttpResponseMessage plantResponse = await plantClient.GetAsync("http://localhost:5190/api/PlantMonitor/GetCurrentLevel");
                bool isNumber = int.TryParse(await plantResponse.Content.ReadAsStringAsync(), out currLevel);
                plantClient.Dispose();
            }
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:5190/api/PlantInfo/CheckLevel");
            string thisThing = response.Content.ReadAsStringAsync().Result.Replace('[',' ').Replace(']',' ');
            PlantData readData = JsonConvert.DeserializeObject<PlantData>(thisThing);
            readData.CurrLevel = Math.Round(((658 - currLevel) / 4.58), 2, MidpointRounding.AwayFromZero);
            client.Dispose();
            if (readData.LowMoisture > readData.CurrLevel)
            {
                SendMail(new MailData(readData.Email, readData.Name, $"Your {readData.PlantName} needs help!", readData.EmailMessage(readData.Name, readData.PlantName, readData.CurrLevel)));
                return currLevel;
            }
            return currLevel;
        }
        // CheckLevels still returns the raw analog data and we do the math on it in line 43
    }

    // custom class to deserialize returned JSON objects
    public class PlantData()
    {
        public int LowMoisture { get; set; }
        public string PlantName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public double CurrLevel { get; internal set; }

        public string EmailMessage(string name, string plantName, double currMoistureLevel)
        {
            return $"Hey {name},\nWe noticed that your {plantName}'s soil has a moisture level reading of {currMoistureLevel}%, which is below the healthy threshold.\n\nPlease water the plant\n\nPlant Info Team";
        }
    }
}
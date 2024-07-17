using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Org.BouncyCastle.Tls;

namespace PlantInfoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantMonitorController : ControllerBase
    {
        private IConfiguration _configuration;
        public PlantMonitorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetCurrentLevel")]
        public async Task<string> GetCurrentLevelAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(""); //insert device IP address
            return await response.Content.ReadAsStringAsync();
        }
    }
}
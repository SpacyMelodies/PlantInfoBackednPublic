
namespace PlantInfoBackend.Services
{
    public class CheckLevelsBGService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(86400000);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://localhost:5190/api/Mail/CheckLevels");
                client.Dispose();
            }
        }
    }
}
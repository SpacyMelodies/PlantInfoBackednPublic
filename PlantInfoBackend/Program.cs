using MailKit;
using Newtonsoft.Json.Serialization;
using PlantInfoBackend;
using PlantInfoBackend.Controllers;
using PlantInfoBackend.Services;
using IMailService = PlantInfoBackend.Services.IMailService;
using MailService = PlantInfoBackend.Services.MailService;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<CheckLevelsBGService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Json Serializer
builder.Services.AddControllers().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(
    options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();
var app = builder.Build();

app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()) ; //REMEMBER TO CHANGE THIS BEFORE PUSHING TO PUBLIC

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

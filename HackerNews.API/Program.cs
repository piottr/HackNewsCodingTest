using HackerNews.Infrastructure.Extensions;
using HackerNews.Infrastructure.HackerNewsClient.Extensions;
using HackerNews.Infrastructure.RedisCaching.Extensions;
using HackerNews.Infrastructure.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddServices(builder.Configuration)
    .AddRedis(builder.Configuration)
    .AddRabbitMq(builder.Configuration)
    .AddHackerNewsClient(builder.Configuration);
 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

var httpsEndpointConfigured = builder.WebHost
    .GetSetting("Kestrel:Certificates:Default:Path") != null;
 
app.UseSwagger();
app.UseSwaggerUI(); 

app.UseAuthorization();

app.MapControllers();

app.Run();

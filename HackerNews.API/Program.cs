using HackerNews.API.Client;
using HackerNews.API.Client.Interfaces;
using HackerNews.API.Common.Config;
using HackerNews.API.Services;
using HackerNews.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<HackerNewsOptions>(builder.Configuration.GetSection("HackerNews"));
builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis:Connection").Value;
    options.InstanceName = "HackerNews_";
});
 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient<IHackerNewsService, HackerNewsService>();
builder.Services.AddScoped<IBestStoriesService, BestStoriesService>();

var app = builder.Build();

var httpsEndpointConfigured = builder.WebHost
    .GetSetting("Kestrel:Certificates:Default:Path") != null;
 
app.UseSwagger();
app.UseSwaggerUI(); 

app.UseAuthorization();

app.MapControllers();

app.Run();

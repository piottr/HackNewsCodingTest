using HackerNews.API.Client;
using HackerNews.API.Client.Interfaces;
using HackerNews.API.Common.Config;
using HackerNews.API.Services;
using HackerNews.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddMemoryCache();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<HackerNewsOptions>(builder.Configuration.GetSection("HackerNews"));

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

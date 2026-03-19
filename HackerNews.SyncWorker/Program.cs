using HackerNews.Infrastructure.Config;
using HackerNews.Infrastructure.HackerNewsClient.Extensions;
using HackerNews.Infrastructure.RabbitMQ.Extensions;
using HackerNews.SyncWorker;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<HackerNewsOptions>(
    builder.Configuration.GetSection("HackerNews"));


builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.AddHackerNewsClient(builder.Configuration);
builder.Services.AddHostedService<HackerNewsWorker>();

var host = builder.Build();

host.Run();

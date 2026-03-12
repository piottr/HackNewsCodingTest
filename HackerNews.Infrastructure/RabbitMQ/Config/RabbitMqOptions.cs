using System;
using System.Collections.Generic;
using System.Text;

namespace HackerNews.Infrastructure.RabbitMQ.Config;

public class RabbitMqOptions
{
    public string HostName { get; set; } = "localhost";
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string ExchangeName { get; set; } = "hackernews.events";
    public string VirtualHost { get; set; } = "/";
}

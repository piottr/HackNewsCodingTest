# HackerNews.API

ASP.NET Core API that fetches the top `n` Hacker News stories sorted by score.

## Infrastructure Stack
- .NET 8 (Web API & Worker Service)
- RabbitMQ as the message broker.
- MassTransit as the abstraction layer for messaging.
- Redis as a distributed cache.
- Docker & Docker Compose for orchestration.

## Run with Docker
 
docker compose up --build

## Docker usage

http://localhost:8080/swagger/index.html

## Possible Enhancements

Add unit Tests
The concurrency limit could be configurable (e.g., via appsettings.json) to allow adjusting it depending on the environment and API limitations.
Also SemaphoreSlim could be replaced with Parallel.ForEachAsync to simplify the code and make it more readable.
Add logging and error handling
Add proper HTTPS support with certificates inside the container
Support optional filtering or maybe additional sorting criteria
...

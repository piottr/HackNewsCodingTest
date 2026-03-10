# HackerNews.API

ASP.NET Core API that fetches the top `n` Hacker News stories sorted by score.

## Run with Docker
 
docker build -t hackernews-api -f HackerNews.API/Dockerfile .
docker run -p 8080:8080 hackernews-api

## Docker usage

http://localhost:8080/swagger/index.html

## Possible Enhancements

Add unit Tests
Add Redis support for caching results and improving performance
The concurrency limit could be configurable (e.g., via appsettings.json) to allow adjusting it depending on the environment and API limitations.
Also SemaphoreSlim could be replaced with Parallel.ForEachAsync to simplify the code and make it more readable.
Add logging and error handling
Add proper HTTPS support with certificates inside the container
Support optional filtering or maybe additional sorting criteria
...

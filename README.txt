# HackerNews.API

ASP.NET Core API that fetches the top `n` Hacker News stories sorted by score.

## Run with Docker
 
docker build -t hackernews-api -f HackerNews.API/Dockerfile .
docker run -p 8080:8080 hackernews-api

## Possible Enhancements

Add unit Tests
Add Redis support for caching results and improving performance
Add logging and error handling
Add proper HTTPS support with certificates inside the container
Support optional filtering or maybe additional sorting criteria
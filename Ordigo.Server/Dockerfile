FROM dotnet:2.2-aspnetcore-runtime-nanoserver-1803
WORKDIR /app
COPY . .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Ordigo.Server.dll
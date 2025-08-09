FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY PIM.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["sh","-c","unset URLS ASPNETCORE_URLS; export ASPNETCORE_HTTP_PORTS=${PORT}; export HTTP_PORTS=${PORT}; exec dotnet PIM.dll"]

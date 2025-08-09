# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY PIM.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app /p:UseAppHost=false

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

# Без URLS; берём порт от Render
ENTRYPOINT ["sh","-c","echo 'ENV CHECK:'; env | grep -E 'PORT|URLS|ASPNET|HTTP_PORTS|HTTPS_PORTS' || true; unset URLS ASPNETCORE_URLS; export HTTP_PORTS=${PORT}; exec dotnet PIM.dll"]
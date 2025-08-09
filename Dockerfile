FROM mcr.microsoft.com/dotnet/sdk:9.0 AS publish
WORKDIR /src
COPY PIM.csproj .
RUN dotnet restore PIM.csproj
COPY . .
RUN dotnet publish PIM.csproj -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["sh","-c","dotnet PIM.dll --urls http://0.0.0.0:${PORT}"]
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApplication1/WebApplication1.csproj", "WebApplication1/"]
COPY ["./Services/Services.csproj", "Services/"]
COPY ["./EfcDatabase/EfcDatabase.csproj", "EfcDatabase/"]
RUN dotnet restore "WebApplication1/WebApplication1.csproj"
RUN dotnet ef migrations add InitialCreate
RUN dotnet ef database update

COPY . .
WORKDIR "/src/WebApplication1"
RUN dotnet build "WebApplication1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplication1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS web-api-image
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplication1.dll"]

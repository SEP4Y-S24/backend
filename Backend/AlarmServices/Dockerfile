FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AlarmServices/AlarmServices.csproj", "AlarmServices/"]
RUN dotnet restore "AlarmServices/AlarmServices.csproj"
COPY . .
WORKDIR "/src/AlarmServices"
RUN dotnet build "AlarmServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AlarmServices.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlarmServices.dll"]

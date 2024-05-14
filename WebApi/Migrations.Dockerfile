FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApplication1.csproj", "./"]

COPY Setup.sh Setup.sh

RUN dotnet tool install --global dotnet-ef --version 7.0.18

RUN dotnet restore "WebApplication1.csproj"
COPY . .
WORKDIR "/src/WebApplication1"

RUN /root/.dotnet/tools/dotnet-ef migrations add InitialMigrations

RUN chmod +x ./Setup.sh
CMD /bin/bash ./Setup.sh

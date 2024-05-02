FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /Services

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /Services
EXPOSE 8080
COPY --from=build-env /Services/out .
ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]


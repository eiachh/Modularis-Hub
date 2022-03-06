# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ModularisWebInterface/*.csproj ./ModularisWebInterface/
RUN dotnet restore ./ModularisWebInterface

# copy everything else and build app
COPY ModularisWebInterface/. ./ModularisWebInterface/
WORKDIR /source/ModularisWebInterface
RUN dotnet build

RUN dotnet publish -c release -o /app --no-restore

#final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ModularisWebInterface.dll"]




# CMD exec /bin/bash -c "trap : TERM INT; sleep infinity & wait"
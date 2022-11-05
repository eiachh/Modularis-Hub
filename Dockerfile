# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ModularisWebInterface/*.csproj ./ModularisWebInterface/
COPY ModuleInstance/*.csproj ./ModuleInstance/
COPY ProjectModularisBot/*.csproj ./ProjectModularisBot/

RUN dotnet restore ./ModuleInstance
RUN dotnet restore ./ModularisWebInterface
RUN dotnet restore ./ProjectModularisBot

# copy everything else and build app
COPY ModuleInstance/. ./ModuleInstance/
COPY ModularisWebInterface/. ./ModularisWebInterface/
COPY ProjectModularisBot/. ./ProjectModularisBot/

WORKDIR /source/ModularisWebInterface
RUN dotnet build
RUN dotnet publish -c release -o /app/ModularisWebInterface --no-restore

WORKDIR /source/ProjectModularisBot
RUN dotnet build
RUN dotnet publish -c release -o /app/ProjectModularisBot --no-restore


#final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR ./app
RUN apt-get update -y
RUN apt-get install curl -y

COPY ./modularisStart.sh ./

WORKDIR /app/ModularisWebInterface
COPY --from=build /app/ModularisWebInterface ./

WORKDIR /app/ProjectModularisBot
COPY --from=build /app/ProjectModularisBot ./

WORKDIR /app
# CMD exec /bin/bash -c "trap : TERM INT; sleep infinity & wait"
ENTRYPOINT ["bash", "./modularisStart.sh"]

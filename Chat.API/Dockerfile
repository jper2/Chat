# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY Chat.API/Chat.API.csproj ./Chat.API/
COPY Chat.Core/Chat.Core.csproj ./Chat.Core/
RUN dotnet restore ./Chat.API/Chat.API.csproj

# Copy the entire source code and build the application
COPY Chat.API ./Chat.API
COPY Chat.Core ./Chat.Core
WORKDIR /app/Chat.API
RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "Chat.API.dll"]

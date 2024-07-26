#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HC.API/HC.API.csproj", "HC.API/"]
COPY ["HC.Application/HC.Application.csproj", "HC.Application/"]
COPY ["HC.Domain/HC.Domain.csproj", "HC.Domain/"]
COPY ["HC.Infrastructure/HC.Infrastructure.csproj", "HC.Infrastructure/"]
RUN dotnet restore "./HC.API/HC.API.csproj"
COPY . .
WORKDIR "/src/HC.API"
RUN dotnet build "./HC.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./HC.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HC.API.dll"]
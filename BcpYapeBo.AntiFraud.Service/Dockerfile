# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BcpYapeBo.AntiFraud.Service/BcpYapeBo.AntiFraud.Service.csproj", "BcpYapeBo.AntiFraud.Service/"]
COPY ["BcpYapeBo.AntiFraud.Application/BcpYapeBo.AntiFraud.Application.csproj", "BcpYapeBo.AntiFraud.Application/"]
COPY ["BcpYapeBo.AntiFraud.Domain/BcpYapeBo.AntiFraud.Domain.csproj", "BcpYapeBo.AntiFraud.Domain/"]
COPY ["BcpYapeBo.AntiFraud.Infrastructure/BcpYapeBo.AntiFraud.Infrastructure.csproj", "BcpYapeBo.AntiFraud.Infrastructure/"]
RUN dotnet restore "./BcpYapeBo.AntiFraud.Service/BcpYapeBo.AntiFraud.Service.csproj"
COPY . .
WORKDIR "/src/BcpYapeBo.AntiFraud.Service"
RUN dotnet build "./BcpYapeBo.AntiFraud.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BcpYapeBo.AntiFraud.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BcpYapeBo.AntiFraud.Service.dll"]
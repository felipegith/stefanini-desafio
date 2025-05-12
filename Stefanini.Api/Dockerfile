FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Stefanini.Api/Stefanini.Api.csproj", "Stefanini.Api/"]
COPY ["Stefanini.Infrastructure/Stefanini.Infrastructure.csproj", "Stefanini.Infrastructure/"]
COPY ["Stefanini.Domain/Stefanini.Domain.csproj", "Stefanini.Domain/"]
COPY ["Stefanini.Application/Stefanini.Application.csproj", "Stefanini.Application/"]
RUN dotnet restore "Stefanini.Api/Stefanini.Api.csproj"
COPY . .
WORKDIR "/src/Stefanini.Api"
RUN dotnet build "./Stefanini.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Stefanini.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stefanini.Api.dll"]

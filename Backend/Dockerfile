#Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["WorldCupPool.Api/WorldCupPool.Api.csproj", "WorldCupPool.Api/"]
COPY ["WorldCupPool.Application/WorldCupPool.Application.csproj", "WorldCupPool.Application/"]
COPY ["WorldCupPool.Domain/WorldCupPool.Domain.csproj", "WorldCupPool.Domain/"]
COPY ["WorldCupPool.Infrastructure/WorldCupPool.Infrastructure.csproj", "WorldCupPool.Infrastructure/"]

RUN dotnet restore "WorldCupPool.Api/WorldCupPool.Api.csproj"

COPY . .
RUN dotnet publish "WorldCupPool.Api/WorldCupPool.Api.csproj" -c Release -o /app/publish


#Stage 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "WorldCupPool.Api.dll"]

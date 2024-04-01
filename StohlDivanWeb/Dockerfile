#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BulkyWeb/StohlDivanWeb.csproj", "BulkyWeb/"]
COPY ["Bulky.DataAccess/StohlDivan.DataAccess.csproj", "Bulky.DataAccess/"]
COPY ["Bulky.Models/StohlDivan.Models.csproj", "Bulky.Models/"]
COPY ["Bulky.Utility/StohlDivan.Utility.csproj", "Bulky.Utility/"]
RUN dotnet restore "./BulkyWeb/StohlDivanWeb.csproj"
COPY . .
WORKDIR "/src/BulkyWeb"
RUN dotnet build "./StohlDivanWeb.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StohlDivanWeb.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StohlDivanWeb.dll"]
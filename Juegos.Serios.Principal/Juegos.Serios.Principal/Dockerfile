#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Juegos.Serios.Principal/Juegos.Serios.Principal.csproj", "Juegos.Serios.Principal/"]
RUN dotnet restore "Juegos.Serios.Principal/Juegos.Serios.Principal.csproj"
COPY . .
WORKDIR "/src/Juegos.Serios.Principal"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENTRYPOINT ["dotnet", "Juegos.Serios.Principal.dll"]
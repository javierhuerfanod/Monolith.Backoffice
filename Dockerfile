# Define la imagen base
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /src
COPY ["Juegos.Serios.Principal/Juegos.Serios.Principal.csproj", "Juegos.Serios.Principal/"]
RUN dotnet restore "Juegos.Serios.Principal/Juegos.Serios.Principal.csproj"
COPY . .
WORKDIR "/src/Juegos.Serios.Principal"
RUN dotnet build "Juegos.Serios.Principal.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Juegos.Serios.Principal.dll"]
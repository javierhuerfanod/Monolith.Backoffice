# Define la imagen base para el proceso de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

# Establece el directorio de trabajo en el contenedor
WORKDIR /src
# Copia el archivo de proyecto CSProj al contenedor
COPY ["Juegos.Serios.Principal/Juegos.Serios.Principal.csproj", "Juegos.Serios.Principal/"]
# Restaura las dependencias definidas en el archivo de proyecto
RUN dotnet restore "Juegos.Serios.Principal/Juegos.Serios.Principal.csproj"
# Copia el resto de los archivos del contexto actual al contenedor
COPY . .
# Establece el directorio de trabajo en el directorio del proyecto principal
WORKDIR "/src/Juegos.Serios.Principal"
# Compila la aplicación en modo Release y la coloca en /app/build
RUN dotnet build "Juegos.Serios.Principal.csproj" -c Release -o /app/build

# Define una nueva etapa de construcción con la imagen base de ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime

# Establece la zona horaria del contenedor
ENV TZ=America/Bogota

# Establece el directorio de trabajo en /app
WORKDIR /app
# Copia los archivos compilados de la etapa de construcción anterior
COPY --from=build /app/build .
# Expone el puerto 80 y 443
EXPOSE 80
EXPOSE 443

# Configura el punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "Juegos.Serios.Principal.dll"]



FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copiamos el proyecto y los archivos de dependencias a la imagen
COPY *.csproj .
RUN dotnet restore

# Copiamos el resto del código fuente a la imagen
COPY . .

# Compilamos la aplicación
RUN dotnet build --configuration Release --output out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copiamos la aplicación compilada desde el paso de compilación
COPY --from=build /app/out ./

# Exponemos el puerto en el que escuchará la aplicación
EXPOSE 4000

# Establecemos la aplicación como el comando que se ejecutará al iniciar el contenedor
ENTRYPOINT ["dotnet", "TelemetryResender.dll"]
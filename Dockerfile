# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto y restaurar dependencias
COPY ["AA1.csproj", "./"]
RUN dotnet restore "AA1.csproj"

# Copiar todo el código fuente
COPY . .

# Compilar la aplicación
RUN dotnet build "AA1.csproj" -c Release -o /app/build

# Etapa 2: Publish
FROM build AS publish
RUN dotnet publish "AA1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Exponer el puerto (usa el puerto según las 4 últimas cifras de tu email)
# Ejemplo: si tu email es a24569@svalero.com, usa el puerto 4569
EXPOSE 6555

# Copiar los archivos publicados desde la etapa publish
COPY --from=publish /app/publish .

# Configurar variable de entorno para el puerto
ENV ASPNETCORE_URLS=http://+:6555

# Punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "AA1.dll"]
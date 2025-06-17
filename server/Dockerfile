# Usa la imagen base de .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Usa la imagen de SDK para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["server.csproj", "./"]
RUN dotnet restore "server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "server.csproj" -c Release -o /app/build

# Publica la aplicación
FROM build AS publish
RUN dotnet publish "server.csproj" -c Release -o /app/publish

# Copia los archivos publicados a la imagen base
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]
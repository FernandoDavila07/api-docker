# Estágio 1: Build (Compilação)
# Usa a imagem do SDK do .NET 8.0 para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o .csproj e restaura os pacotes (NuGet)
COPY *.csproj .
RUN dotnet restore

# Copia o resto do código e compila o projeto
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: Runtime (Execução)
# Usa a imagem de runtime (menor) para rodar a API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

# Copia os arquivos compilados do estágio de build
COPY --from=build /app/publish .

# Define como iniciar a API (troque 'MinhaApi.dll' se seu projeto tiver outro nome)
ENTRYPOINT ["dotnet", "my-api.dll"]
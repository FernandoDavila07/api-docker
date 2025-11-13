# Estágio 1: Build (Compilação)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia e restaura pacotes
COPY *.csproj .
RUN dotnet restore

# Copia o código e publica
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: Runtime (Execução)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# --- INSTALAÇÃO DO DATADOG TRACER ---
# 1. Troca para root para instalar dependências
USER root

# 2. Instala curl
RUN apt-get update && apt-get install -y curl && \
    rm -rf /var/lib/apt/lists/*

# 3. Cria a pasta /opt (se não existir) e baixa o profiler direto do GitHub
# Usamos curl -L (seguir redirecionamentos) e -f (falhar se der erro HTTP)
RUN mkdir -p /opt && \
    curl -L -f https://github.com/DataDog/datadog-dotnet-apm/releases/download/v2.61.0/datadog-dotnet-apm-2.61.0-linux-x64.tar.gz -o datadog-dotnet-apm.tar.gz && \
    tar -xzf datadog-dotnet-apm.tar.gz -C /opt && \
    rm datadog-dotnet-apm.tar.gz

# 4. Configura as variáveis de ambiente para ATIVAR o profiler
ENV CORECLR_ENABLE_PROFILING="1"
ENV CORECLR_PROFILER="{846F5F1C-F9AE-4B07-969E-05C26BC060D8}"
ENV CORECLR_PROFILER_PATH="/opt/Datadog.Trace.ClrProfiler.Native.so"
ENV DD_DOTNET_TRACER_HOME="/opt"

# 5. Volta para o usuário seguro
USER $APP_UID
# ------------------------------------

# Expõe a porta e inicia
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "my-api.dll"]
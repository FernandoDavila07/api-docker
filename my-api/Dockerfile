# Estágio 1: Build (sem mudanças)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: Runtime (com a CORREÇÃO)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# --- INSTALAÇÃO DO DATADOG TRACER ---
# 1. Troca para usuário root para poder instalar pacotes
USER root

# 2. Instala 'curl' (para baixar)
RUN apt-get update && apt-get install -y curl && \
    rm -rf /var/lib/apt/lists/*

# 3. Baixa e descompacta o profiler do Datadog (DA URL CORRETA)
RUN curl -L https://dl.ddtrace.com/datadog-dotnet-apm/stable/datadog-dotnet-apm-2.61.0-linux-x64.tar.gz -o datadog-dotnet-apm.tar.gz && \
    tar -xzf datadog-dotnet-apm.tar.gz -C /opt && \
    rm datadog-dotnet-apm.tar.gz
    
# 4. Define as variáveis de ambiente para LIGAR o profiler
ENV CORECLR_ENABLE_PROFILING="1"
ENV CORECLR_PROFILER="{846F5F1C-F9AE-4B07-969E-05C26BC060D8}"
ENV CORECLR_PROFILER_PATH="/opt/Datadog.Trace.ClrProfiler.Native.so"
ENV DD_DOTNET_TRACER_HOME="/opt"

# 5. Volta para o usuário padrão do container (mais seguro)
USER $APP_UID
# ------------------------------------

EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "my-api.dll"]
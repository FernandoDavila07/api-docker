# Estágio 1: Build (sem mudanças)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# --- INSTALAÇÃO DO DATADOG TRACER ---
USER root

# 1. Instala curl
RUN apt-get update && apt-get install -y curl && \
    rm -rf /var/lib/apt/lists/*

# 2. Baixa a versão v2.51.0 (QUE EXISTE) do GitHub
RUN mkdir -p /opt && \
    curl -L -f https://github.com/DataDog/datadog-dotnet-apm/releases/download/v2.51.0/datadog-dotnet-apm-2.51.0-linux-x64.tar.gz -o datadog-dotnet-apm.tar.gz && \
    tar -xzf datadog-dotnet-apm.tar.gz -C /opt && \
    rm datadog-dotnet-apm.tar.gz

# 3. Configura as variáveis
ENV CORECLR_ENABLE_PROFILING="1"
ENV CORECLR_PROFILER="{846F5F1C-F9AE-4B07-969E-05C26BC060D8}"
ENV CORECLR_PROFILER_PATH="/opt/Datadog.Trace.ClrProfiler.Native.so"
ENV DD_DOTNET_TRACER_HOME="/opt"

USER $APP_UID
# ------------------------------------

EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "my-api.dll"]
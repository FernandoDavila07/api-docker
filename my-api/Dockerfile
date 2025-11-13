# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# --- A MÁGICA DO APM (INSTALADOR OFICIAL) ---
USER root

# 1. Instala curl
RUN apt-get update && apt-get install -y curl && \
    rm -rf /var/lib/apt/lists/*

# 2. Baixa e instala o Datadog APM (.deb)
# Como seu DNS agora funciona, isso vai baixar em segundos!
RUN curl -L -f https://github.com/DataDog/datadog-dotnet-apm/releases/download/v2.51.0/datadog-dotnet-apm_2.51.0_amd64.deb -o datadog-apm.deb && \
    dpkg -i datadog-apm.deb && \
    rm datadog-apm.deb

# 3. Variáveis de Ambiente para ATIVAR o espião
ENV CORECLR_ENABLE_PROFILING="1"
ENV CORECLR_PROFILER="{846F5F1C-F9AE-4B07-969E-05C26BC060D8}"
ENV CORECLR_PROFILER_PATH="/opt/datadog/Datadog.Trace.ClrProfiler.Native.so"
ENV DD_DOTNET_TRACER_HOME="/opt/datadog"

USER $APP_UID
# --------------------------------------------

EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "my-api.dll"]
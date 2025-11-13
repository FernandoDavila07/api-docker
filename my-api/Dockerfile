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

# --- INSTALAÇÃO MANUAL (OFFLINE) ---
USER root

# 1. Copia o arquivo da sua pasta para dentro da imagem
COPY datadog-apm.deb .

# 2. Instala usando o arquivo local
RUN dpkg -i datadog-apm.deb && \
    rm datadog-apm.deb

# 3. Configura as variáveis para o Datadog v3
ENV CORECLR_ENABLE_PROFILING="1"
ENV CORECLR_PROFILER="{846F5F1C-F9AE-4B07-969E-05C26BC060D8}"
ENV CORECLR_PROFILER_PATH="/opt/datadog/Datadog.Trace.ClrProfiler.Native.so"
ENV DD_DOTNET_TRACER_HOME="/opt/datadog"

USER $APP_UID
# -----------------------------------

EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "my-api.dll"]
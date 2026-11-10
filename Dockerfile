FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Копируем файл решения и проект
COPY ["StatML.sln", "./"]
COPY ["StatML/StatML.csproj", "StatML/"]
RUN dotnet restore "StatML/StatML.csproj"

COPY . .
WORKDIR "/src/StatML"
RUN dotnet build "StatML.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StatML.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .

# === ДОБАВЛЯЕМ УСТАНОВКУ PLAYWRIGHT И БРАУЗЕРА ===
RUN dotnet tool install --global Microsoft.Playwright.CLI
ENV PATH="$PATH:/root/.dotnet/tools"
RUN playwright install --with-deps
# ===============================================

ENTRYPOINT ["dotnet", "StatML.dll"]

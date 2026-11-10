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

# Копируем скомпилированное приложение
COPY --from=publish /app/publish .

# Устанавливаем PowerShell (нужен для запуска скрипта playwright.ps1 в aspnet-образе)
RUN apt-get update && apt-get install -y wget curl libicu-dev && \
    wget -q https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && apt-get install -y powershell && \
    rm packages-microsoft-prod.deb

# Запускаем скрипт установки браузеров Playwright со всеми системными зависимостями прямо в финальном образе
RUN pwsh /app/playwright.ps1 install --with-deps

ENTRYPOINT ["dotnet", "StatML.dll"]

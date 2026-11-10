FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Копируем файл решения и проект
COPY ["StatML.sln", "./"]
COPY ["StatML/StatML.csproj", "StatML/"]
RUN dotnet restore "StatML/StatML.csproj"

COPY . .
WORKDIR "/src/StatML"
RUN dotnet build "StatML.csproj" -c Release -o /app/build

# Устанавливаем Playwright CLI и загружаем браузеры с зависимостями прямо на этапе сборки SDK
RUN dotnet tool install --global Microsoft.Playwright.CLI
ENV PATH="$PATH:/root/.dotnet/tools"
RUN playwright install --with-deps

FROM build AS publish
RUN dotnet publish "StatML.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Копируем скомпилированное приложение
COPY --from=publish /app/publish .

# Копируем установленные браузеры Playwright из кэша сборки SDK
COPY --from=build /root/.cache/ms-playwright /root/.cache/ms-playwright

ENTRYPOINT ["dotnet", "StatML.dll"]

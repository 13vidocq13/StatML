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

ENTRYPOINT ["dotnet", "StatML.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AutoMonitoring.Presentation/AutoMonitoring.Presentation.csproj", "AutoMonitoring.Presentation/"]
COPY ["AutoMonitoring.BLL/AutoMonitoring.BLL.csproj", "AutoMonitoring.BLL/"]
COPY ["AutoMonitoring.Domain/AutoMonitoring.Domain.csproj", "AutoMonitoring.Domain/"]
COPY ["AutoMonitoring.DAL/AutoMonitoring.DAL.csproj", "AutoMonitoring.DAL/"]
RUN dotnet restore "AutoMonitoring.Presentation/AutoMonitoring.Presentation.csproj"
COPY . .
WORKDIR "/src/AutoMonitoring.Presentation"
RUN dotnet build "AutoMonitoring.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AutoMonitoring.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoMonitoring.Presentation.dll"]

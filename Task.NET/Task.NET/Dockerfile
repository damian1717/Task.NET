FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV HOME /root
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Task.NET/Task.NET.csproj", "Task.NET/"]
RUN dotnet restore "./Task.NET/Task.NET.csproj"
COPY . .
WORKDIR "/src/Task.NET"
RUN dotnet build "./Task.NET.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Task.NET.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Task.NET.dll"]
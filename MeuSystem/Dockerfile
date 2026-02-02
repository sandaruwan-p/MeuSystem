FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_HTTP_PORTS=8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["MeuSystem/MeuSystem.csproj", "MeuSystem/"]
RUN dotnet restore "MeuSystem/MeuSystem.csproj"
COPY . .
WORKDIR "/src/MeuSystem"
RUN dotnet build "MeuSystem.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "MeuSystem.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeuSystem.dll"]

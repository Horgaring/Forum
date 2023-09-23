FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Forum/Api/Api.csproj", "Services/Forum/Api/"]
COPY ["Services/Forum/Infrastructure/Infrastructure.csproj", "Services/Forum/Infrastructure/"]
COPY ["Services/Forum/Domain/Domain.csproj", "Services/Forum/Domain/"]
COPY ["BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/"]
COPY ["Services/Forum/Application/Application.csproj", "Services/Forum/Application/"]
RUN dotnet restore "Services/Forum/Api/Api.csproj"
COPY . .
WORKDIR "/src/Services/Forum/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]

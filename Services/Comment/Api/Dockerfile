FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Comment/Api/Api.csproj", "Services/Comment/Api/"]
COPY ["Services/Comment/Application/Application.csproj", "Services/Comment/Application/"]
COPY ["Services/Comment/Domain/Domain.csproj", "Services/Comment/Domain/"]
COPY ["Services/Comment/Infrastructure/Infrastructure.csproj", "Services/Comment/Infrastructure/"]
COPY ["BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/"]
RUN dotnet restore "Services/Comment/Api/Api.csproj"
COPY . .
WORKDIR "/src/Services/Comment/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]

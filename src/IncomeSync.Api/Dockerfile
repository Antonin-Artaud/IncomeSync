FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/IncomeSync.Api/IncomeSync.Api.csproj", "src/IncomeSync.Api/"]
COPY ["src/IncomeSync.Core.Shared/IncomeSync.Core.Shared.csproj", "src/IncomeSync.Core.Shared/"]
COPY ["src/IncomeSync.Core/IncomeSync.Core.csproj", "src/IncomeSync.Core/"]
COPY ["src/IncomeSync.Persistence/IncomeSync.Persistence.csproj", "src/IncomeSync.Persistence/"]
RUN dotnet restore "src/IncomeSync.Api/IncomeSync.Api.csproj"
COPY . .
WORKDIR "/src/src/IncomeSync.Api"
RUN dotnet build "IncomeSync.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IncomeSync.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IncomeSync.Api.dll"]

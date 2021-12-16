FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS dotnet_restore

COPY ["CovidVaccineSchedulesQueryApi.sln", "CovidVaccineSchedulesQueryApi.sln"]
COPY ["src/CovidVaccineSchedulesQueryApi.Api/CovidVaccineSchedulesQueryApi.Api.csproj", "src/CovidVaccineSchedulesQueryApi.Api/CovidVaccineSchedulesQueryApi.Api.csproj"]
COPY ["src/CovidVaccineSchedulesQueryApi.Core/CovidVaccineSchedulesQueryApi.Core.csproj", "src/CovidVaccineSchedulesQueryApi.Core/CovidVaccineSchedulesQueryApi.Core.csproj"]
COPY ["src/CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb/CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.csproj", "src/CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb/CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.csproj"]
COPY ["src/CovidVaccineSchedulesQueryApi.Infra.IoC/CovidVaccineSchedulesQueryApi.Infra.IoC.csproj", "src/CovidVaccineSchedulesQueryApi.Infra.IoC/CovidVaccineSchedulesQueryApi.Infra.IoC.csproj"]
COPY ["src/CovidVaccineSchedulesQueryApi.Infra.Logging/CovidVaccineSchedulesQueryApi.Infra.Logging.csproj", "src/CovidVaccineSchedulesQueryApi.Infra.Logging/CovidVaccineSchedulesQueryApi.Infra.Logging.csproj"]
COPY ["test/CovidVaccineSchedulesQueryApi.Api.Test/CovidVaccineSchedulesQueryApi.Api.Test.csproj", "test/CovidVaccineSchedulesQueryApi.Api.Test/CovidVaccineSchedulesQueryApi.Api.Test.csproj"]
COPY ["test/CovidVaccineSchedulesQueryApi.Core.Test/CovidVaccineSchedulesQueryApi.Core.Test.csproj", "test/CovidVaccineSchedulesQueryApi.Core.Test/CovidVaccineSchedulesQueryApi.Core.Test.csproj"]
COPY ["test/CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Test/CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Test.csproj", "test/CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Test/CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Test.csproj"]
COPY ["test/CovidVaccineSchedulesQueryApi.Infra.Logging.Test/CovidVaccineSchedulesQueryApi.Infra.Logging.Test.csproj", "test/CovidVaccineSchedulesQueryApi.Infra.Logging.Test/CovidVaccineSchedulesQueryApi.Infra.Logging.Test.csproj"]
COPY ["test/CovidVaccineSchedulesQueryApi.IntegrationTest/CovidVaccineSchedulesQueryApi.IntegrationTest.csproj", "test/CovidVaccineSchedulesQueryApi.IntegrationTest/CovidVaccineSchedulesQueryApi.IntegrationTest.csproj"]
RUN dotnet restore "CovidVaccineSchedulesQueryApi.sln" --no-cache

FROM dotnet_restore AS dotnet_publish
WORKDIR /app
COPY . .
RUN dotnet publish "src/CovidVaccineSchedulesQueryApi.Api/CovidVaccineSchedulesQueryApi.Api.csproj" -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /out
COPY --from=dotnet_publish /out .

EXPOSE 80

ENTRYPOINT ["dotnet", "CovidVaccineSchedulesQueryApi.Api.dll"]

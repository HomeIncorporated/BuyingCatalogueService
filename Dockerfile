FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /app

# Copy application projects
COPY *.sln .
COPY src/NHSD.BuyingCatalogue.Contracts/*.csproj ./src/NHSD.BuyingCatalogue.Contracts/
COPY src/NHSD.BuyingCatalogue.API/*.csproj ./src/NHSD.BuyingCatalogue.API/
COPY src/NHSD.BuyingCatalogue.Capabilities.API/*.csproj ./src/NHSD.BuyingCatalogue.Capabilities.API/
COPY src/NHSD.BuyingCatalogue.Capabilities.Application/*.csproj ./src/NHSD.BuyingCatalogue.Capabilities.Application/
COPY src/NHSD.BuyingCatalogue.SolutionLists.API/*.csproj ./src/NHSD.BuyingCatalogue.SolutionLists.API/
COPY src/NHSD.BuyingCatalogue.SolutionLists.Application/*.csproj ./src/NHSD.BuyingCatalogue.SolutionLists.Application/
COPY src/NHSD.BuyingCatalogue.Solutions.API/*.csproj ./src/NHSD.BuyingCatalogue.Solutions.API/
COPY src/NHSD.BuyingCatalogue.Solutions.Application/*.csproj ./src/NHSD.BuyingCatalogue.Solutions.Application/
COPY src/NHSD.BuyingCatalogue.Application/*.csproj ./src/NHSD.BuyingCatalogue.Application/
COPY src/NHSD.BuyingCatalogue.Infrastructure/*.csproj ./src/NHSD.BuyingCatalogue.Infrastructure/
COPY src/NHSD.BuyingCatalogue.Persistence/*.csproj ./src/NHSD.BuyingCatalogue.Persistence/
COPY src/NHSD.BuyingCatalogue.Data/*.csproj ./src/NHSD.BuyingCatalogue.Data/

# Copy test projects
COPY tests/NHSD.BuyingCatalogue.API.IntegrationTests/*.csproj ./tests/NHSD.BuyingCatalogue.API.IntegrationTests/
COPY tests/NHSD.BuyingCatalogue.API.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.API.UnitTests/
COPY tests/NHSD.BuyingCatalogue.Capabilities.API.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.Capabilities.API.UnitTests/
COPY tests/NHSD.BuyingCatalogue.Capabilities.Application.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.Capabilities.Application.UnitTests/
COPY tests/NHSD.BuyingCatalogue.SolutionLists.API.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.SolutionLists.API.UnitTests/
COPY tests/NHSD.BuyingCatalogue.SolutionLists.Application.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.SolutionLists.Application.UnitTests/
COPY tests/NHSD.BuyingCatalogue.Solution.API.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.Solution.API.UnitTests/
COPY tests/NHSD.BuyingCatalogue.Application.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.Application.UnitTests/
COPY tests/NHSD.BuyingCatalogue.Data.Tests/*.csproj ./tests/NHSD.BuyingCatalogue.Data.Tests/
COPY tests/NHSD.BuyingCatalogue.Persistence.DatabaseTests/*.csproj ./tests/NHSD.BuyingCatalogue.Persistence.DatabaseTests/
COPY tests/NHSD.BuyingCatalogue.Infrastructure.Tests/*.csproj ./tests/NHSD.BuyingCatalogue.Infrastructure.Tests/
COPY tests/NHSD.BuyingCatalogue.Testing.Data/*.csproj ./tests/NHSD.BuyingCatalogue.Testing.Data/
COPY tests/NHSD.BuyingCatalogue.Testing.Tools/*.csproj ./tests/NHSD.BuyingCatalogue.Testing.Tools/

# Restore main application and the test unit test project
RUN dotnet restore

# Copy full solution over
COPY . .
RUN dotnet build
# Publish the API
FROM build AS publish
WORKDIR /app/src/NHSD.BuyingCatalogue.API
RUN dotnet publish -c Release -o out

# Run the API
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS runtime
WORKDIR /app
COPY --from=publish /app/src/NHSD.BuyingCatalogue.API/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "NHSD.BuyingCatalogue.API.dll"]

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy all csproj files and restore to leverage build caching
COPY ["LoanSystem.Api/LoanSystem.Api.csproj", "LoanSystem.Api/"]
COPY ["LoanSystem.Application/LoanSystem.Application.csproj", "LoanSystem.Application/"]
COPY ["LoanSystem.Domain/LoanSystem.Domain.csproj", "LoanSystem.Domain/"]
COPY ["LoanSystem.Infrastructure/LoanSystem.Infrastructure.csproj", "LoanSystem.Infrastructure/"]
RUN dotnet restore "LoanSystem.Api/LoanSystem.Api.csproj"

# Copy everything else and publish the API project
COPY . .
WORKDIR "/src/LoanSystem.Api"
RUN dotnet publish "LoanSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Port configuration
ENV PORT=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "LoanSystem.Api.dll"]

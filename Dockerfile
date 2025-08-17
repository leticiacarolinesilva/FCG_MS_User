# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/FCG_MS_Users.Api/FCG_MS_Users.Api.csproj", "src/FCG_MS_Users.Api/"]
COPY ["src/FCG_MS_Users.Application/FCG_MS_Users.Application.csproj", "src/FCG_MS_Users.Application/"]
COPY ["src/FCG_MS_Users.Domain/FCG_MS_Users.Domain.csproj", "src/FCG_MS_Users.Domain/"]
COPY ["src/FCG_MS_Users.Infrastructure/FCG_MS_Users.Infrastructure.csproj", "src/FCG_MS_Users.Infrastructure/"]
COPY ["src/FCG_MS_Users.Tests/FCG_MS_Users.Tests.csproj", "src/FCG_MS_Users.Tests/"]

# Restore packages
RUN dotnet restore "src/FCG_MS_Users.Api/FCG_MS_Users.Api.csproj"
RUN dotnet restore "src/FCG_MS_Users.Tests/FCG_MS_Users.Tests.csproj"

# Copy the rest of the code
COPY . .

# Build the projects
RUN dotnet build "src/FCG_MS_Users.Api/FCG_MS_Users.Api.csproj" -c Release
RUN dotnet build "src/FCG_MS_Users.Tests/FCG_MS_Users.Tests.csproj" -c Release

# Test stage
FROM build AS test
WORKDIR /src
RUN dotnet test "src/FCG_MS_Users.Tests/FCG_MS_Users.Tests.csproj" -c Release --no-build
# Build and publish
FROM build AS publish
RUN dotnet build "src/FCG_MS_Users.Api/FCG_MS_Users.Api.csproj" -c Release -o /app/build
RUN dotnet publish "src/FCG_MS_Users.Api/FCG_MS_Users.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Copy the published app
COPY --from=publish /app/publish .

# Create a non-root user
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Expose port 80
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "FCG_MS_Users.Api.dll"]

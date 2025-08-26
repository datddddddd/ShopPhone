# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ solution
COPY . .

# Restore dependencies
RUN dotnet restore "./ShopPhone.sln"

# Build và publish project
RUN dotnet publish "./ShopPhone/ShopPhone.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ShopPhone.dll"]

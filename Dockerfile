# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["SmartRecipe.Api.csproj", "./"]
RUN dotnet restore "./SmartRecipe.Api.csproj"

# Copy everything else and build the app
COPY . .
RUN dotnet publish "SmartRecipe.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Tell .NET to listen on port 8080 (Render's default expectation for containers)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "SmartRecipe.Api.dll"]

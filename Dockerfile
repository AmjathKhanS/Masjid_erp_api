# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["PersonalDetailsAPI.csproj", "./"]
RUN dotnet restore "PersonalDetailsAPI.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "PersonalDetailsAPI.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "PersonalDetailsAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copy published app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PersonalDetailsAPI.dll"]

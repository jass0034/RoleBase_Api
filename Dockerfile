# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["RoleBase_Api/RoleBase_Api.csproj", "RoleBase_Api/"]
RUN dotnet restore "RoleBase_Api/RoleBase_Api.csproj"

COPY . .
WORKDIR "/src/RoleBase_Api"
RUN dotnet build "RoleBase_Api.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "RoleBase_Api.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "RoleBase_Api.dll"]

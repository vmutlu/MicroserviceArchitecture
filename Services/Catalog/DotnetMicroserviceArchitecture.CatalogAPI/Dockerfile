FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/DotnetMicroserviceArchitecture.Core/*.csproj Shared/DotnetMicroserviceArchitecture.Core/
COPY Services/Catalog/DotnetMicroserviceArchitecture.CatalogAPI/*.csproj Services/Catalog/DotnetMicroserviceArchitecture.CatalogAPI/
RUN dotnet restore Services/Catalog/DotnetMicroserviceArchitecture.CatalogAPI/*.csproj
COPY . .
RUN dotnet publish Services/Catalog/DotnetMicroserviceArchitecture.CatalogAPI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","DotnetMicroserviceArchitecture.CatalogAPI.dll" ]
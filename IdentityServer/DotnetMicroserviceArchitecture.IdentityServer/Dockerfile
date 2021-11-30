FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/DotnetMicroserviceArchitecture.Core/*.csproj Shared/DotnetMicroserviceArchitecture.Core/
COPY IdentityServer/DotnetMicroserviceArchitecture.IdentityServer/*.csproj IdentityServer/DotnetMicroserviceArchitecture.IdentityServer/
RUN dotnet restore IdentityServer/DotnetMicroserviceArchitecture.IdentityServer/*.csproj
COPY . .
RUN dotnet publish IdentityServer/DotnetMicroserviceArchitecture.IdentityServer/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","DotnetMicroserviceArchitecture.IdentityServer.dll" ]
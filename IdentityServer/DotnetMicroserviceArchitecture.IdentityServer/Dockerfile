FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app
EXPOSE 80
COPY Shared/DotnetMicroserviceArchitecture.Core/*.csproj Shared/DotnetMicroserviceArchitecture.Core/
COPY IdentityServer/DotnetMicroserviceArchitecture.IdentityServer/*.csproj IdentityServer/DotnetMicroserviceArchitecture.IdentityServer/
RUN dotnet restore IdentityServer/DotnetMicroserviceArchitecture.IdentityServer/*.csproj
COPY . .
RUN dotnet publish IdentityServer/DotnetMicroserviceArchitecture.IdentityServer/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","DotnetMicroserviceArchitecture.IdentityServer.dll" ]
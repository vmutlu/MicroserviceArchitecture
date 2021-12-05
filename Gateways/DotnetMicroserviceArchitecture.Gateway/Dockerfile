FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Gateways/DotnetMicroserviceArchitecture.Gateway/*.csproj Gateways/DotnetMicroserviceArchitecture.Gateway/
RUN dotnet restore Gateways/DotnetMicroserviceArchitecture.Gateway/*.csproj 
COPY . .
RUN dotnet publish Gateways/DotnetMicroserviceArchitecture.Gateway/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","DotnetMicroserviceArchitecture.Gateway.dll" ]
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/DotnetMicroserviceArchitecture.Core/*.csproj Shared/DotnetMicroserviceArchitecture.Core/
COPY Services/Order/DotnetMicroserviceArchitecture.OrderAPI/*.csproj Services/Order/DotnetMicroserviceArchitecture.OrderAPI/
COPY Services/Order/DotnetMicroserviceArchitecture.Core/*.csproj Services/Order/DotnetMicroserviceArchitecture.Core/
COPY Services/Order/DotnetMicroserviceArchitecture.Domain/*.csproj Services/Order/DotnetMicroserviceArchitecture.Domain/
COPY Services/Order/DotnetMicroserviceArchitecture.Infrastructure/*.csproj Services/Order/DotnetMicroserviceArchitecture.Infrastructure/
COPY Services/Order/DotnetMicroserviceArchitecture.Application/*.csproj Services/Order/DotnetMicroserviceArchitecture.Application/
RUN dotnet restore Services/Order/DotnetMicroserviceArchitecture.OrderAPI/*.csproj
COPY . .
RUN dotnet publish Services/Order/DotnetMicroserviceArchitecture.OrderAPI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","DotnetMicroserviceArchitecture.OrderAPI.dll" ]
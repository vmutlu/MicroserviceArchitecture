FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/DotnetMicroserviceArchitecture.Core/*.csproj Shared/DotnetMicroserviceArchitecture.Core/
COPY Services/Basket/DotnetMicroserviceArchitecture.BasketAPI/*.csproj Services/Basket/DotnetMicroserviceArchitecture.BasketAPI/
RUN dotnet restore Services/Basket/DotnetMicroserviceArchitecture.BasketAPI/*.csproj
COPY . .
RUN dotnet publish Services/Basket/DotnetMicroserviceArchitecture.BasketAPI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","DotnetMicroserviceArchitecture.BasketAPI.dll" ]
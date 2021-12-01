FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/DotnetMicroserviceArchitecture.Core/*.csproj Shared/DotnetMicroserviceArchitecture.Core/
COPY Services/Discount/DotnetMicroserviceArchitecture.DiscountAPI/*.csproj Services/Discount/DotnetMicroserviceArchitecture.DiscountAPI/
RUN dotnet restore Services/Discount/DotnetMicroserviceArchitecture.DiscountAPI/*.csproj
COPY . .
RUN dotnet publish Services/Discount/DotnetMicroserviceArchitecture.DiscountAPI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","DotnetMicroserviceArchitecture.DiscountAPI.dll" ]
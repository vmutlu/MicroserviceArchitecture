FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/DotnetMicroserviceArchitecture.Core/*.csproj Shared/DotnetMicroserviceArchitecture.Core/
COPY UI/DotnetMicroserviceArchitecture.UI/*.csproj UI/DotnetMicroserviceArchitecture.UI/
RUN dotnet restore UI/DotnetMicroserviceArchitecture.UI/*.csproj
COPY . .
RUN dotnet publish UI/DotnetMicroserviceArchitecture.UI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","DotnetMicroserviceArchitecture.UI.dll" ]
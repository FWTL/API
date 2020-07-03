FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

COPY . /src
WORKDIR /src
RUN dotnet restore FWTL.Auth
RUN dotnet publish FWTL.Auth --configuration Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

 WORKDIR /app
 COPY --from=build /publish .
 ENTRYPOINT ["dotnet", "FWTL.Auth.dll"]
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./

RUN dotnet restore ArtyApp.sln

RUN dotnet publish -r linux-x64 -c Release -o out ArtyApp.sln

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "ArtyApp.Api.dll"]
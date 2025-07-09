FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY IndgImageProcessor.sln ./
COPY Indg.ImageAPI/*.csproj Indg.ImageAPI/
COPY Indg.DataAccess/*.csproj Indg.DataAccess/
COPY Indg.Services/*.csproj Indg.Services/
COPY ImageAPI.Tests/*.csproj ImageAPI.Tests/

RUN dotnet restore

COPY Indg.ImageAPI/. Indg.ImageAPI/
COPY Indg.DataAccess/. Indg.DataAccess/
COPY Indg.Services/. Indg.Services/
COPY ImageAPI.Tests/. ImageAPI.Tests/

WORKDIR /src/Indg.ImageAPI
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Indg.ImageAPI.dll"]

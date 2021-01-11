# Environment for building project
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c release -r linux-x64 -o out --self-contained=false

# Environment for runtime
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ScorpPlusBackend.dll"]
# Use the official .NET image as a build stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["AuthenticationAPI/AuthenticationAPI.csproj", "AuthenticationAPI/"]
RUN dotnet restore "AuthenticationAPI/AuthenticationAPI.csproj"
COPY AuthenticationAPI/. ./AuthenticationAPI/
WORKDIR "/src/AuthenticationAPI"
RUN dotnet build "AuthenticationAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthenticationAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationAPI.dll"]

# NEEDS TO BE RUN FROM PARENT DIRECTORY TO INCLUDE PROTOS DIRECTORY
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebClient/WebClient.csproj", "WebClient/WebClient.csproj"]
RUN dotnet restore "WebClient/WebClient.csproj"
COPY ["WebClient", "WebClient"]
COPY ["Protos", "Protos"]
RUN dotnet build "WebClient/WebClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebClient/WebClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebClient.dll"]
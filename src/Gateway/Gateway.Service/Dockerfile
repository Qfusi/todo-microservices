# NEEDS TO BE RUN FROM PARENT DIRECTORY TO INCLUDE PROTOS DIRECTORY
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./Gateway/Gateway.Service/Gateway.Service.csproj", "./Gateway/Gateway.Service/Gateway.Service.csproj"]
RUN dotnet restore "./Gateway/Gateway.Service/Gateway.Service.csproj"
COPY ["./Gateway/Gateway.Service", "./Gateway/Gateway.Service"]
COPY ["./Protos", "./Protos"]
RUN dotnet build "./Gateway/Gateway.Service/Gateway.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Gateway/Gateway.Service/Gateway.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.Service.dll"]
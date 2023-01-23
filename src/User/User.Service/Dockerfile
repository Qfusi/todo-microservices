# NEEDS TO BE RUN FROM PARENT DIRECTORY TO INCLUDE PROTOS DIRECTORY
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./User/User.Service/User.Service.csproj", "./User/User.Service/User.Service.csproj"]
RUN dotnet restore "./User/User.Service/User.Service.csproj"
COPY ["./User/User.Service", "./User/User.Service"]
COPY ["./Protos", "./Protos"]
RUN dotnet build "./User/User.Service/User.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./User/User.Service/User.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.Service.dll"]
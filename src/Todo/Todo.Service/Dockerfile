# NEEDS TO BE RUN FROM PARENT DIRECTORY TO INCLUDE PROTOS DIRECTORY
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./Todo/Todo.Service/Todo.Service.csproj", "./Todo/Todo.Service/Todo.Service.csproj"]
RUN dotnet restore "./Todo/Todo.Service/Todo.Service.csproj"
COPY ["./Todo/Todo.Service", "./Todo/Todo.Service"]
COPY ["./Protos", "./Protos"]
RUN dotnet build "./Todo/Todo.Service/Todo.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Todo/Todo.Service/Todo.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Todo.Service.dll"]
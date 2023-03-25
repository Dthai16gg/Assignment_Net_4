#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Colo_Shop/Colo_Shop.csproj", "Colo_Shop/"]
RUN dotnet restore "Colo_Shop/Colo_Shop.csproj"
COPY . .
WORKDIR "/src/Colo_Shop"
RUN dotnet build "Colo_Shop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Colo_Shop.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Colo_Shop.dll"]
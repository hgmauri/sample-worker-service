#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Sample.WorkerService.Api/Sample.WorkerService.Api.csproj", "src/Sample.WorkerService.Api/"]
COPY ["src/Sample.WorkerService.Core/Sample.WorkerService.Core.csproj", "src/Sample.WorkerService.Core/"]
RUN dotnet restore "src/Sample.WorkerService.Api/Sample.WorkerService.Api.csproj"
COPY . .
WORKDIR "/src/src/Sample.WorkerService.Api"
RUN dotnet build "Sample.WorkerService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.WorkerService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.WorkerService.Api.dll"]

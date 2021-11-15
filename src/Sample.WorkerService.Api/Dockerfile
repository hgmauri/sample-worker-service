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

ENV TZ=America/Sao_Paulo
ENV LANG pt-BR
ENV LANGUAGE pt-BR
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.WorkerService.Api.dll"]

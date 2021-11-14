FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Sample.WorkerService/Sample.WorkerService.csproj", "src/Sample.WorkerService/"]
COPY ["src/Sample.WorkerService.Core/Sample.WorkerService.Core.csproj", "src/Sample.WorkerService.Core/"]
RUN dotnet restore "src/Sample.WorkerService/Sample.WorkerService.csproj"
COPY . .
WORKDIR "/src/src/Sample.WorkerService"
RUN dotnet build "Sample.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.WorkerService.csproj" -c Release -o /app/publish

ENV TZ=America/Sao_Paulo
ENV LANG pt-BR
ENV LANGUAGE pt-BR
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.WorkerService.dll"]
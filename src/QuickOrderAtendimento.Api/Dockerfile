#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["QuickOrderAtendimento.Api/QuickOrderAtendimento.Api.csproj", "QuickOrderAtendimento.Api/"]
COPY ["QuickOrderAtendimento.Application/QuickOrderAtendimento.Application.csproj", "QuickOrderAtendimento.Application/"]
COPY ["QuickOrderAtendimento.Domain/QuickOrderAtendimento.Domain.csproj", "QuickOrderAtendimento.Domain/"]
COPY ["QuickOrderAtendimento.Infra.Gateway/QuickOrderAtendimento.Infra.Gateway.csproj", "QuickOrderAtendimento.Infra.Gateway/"]
COPY ["QuickOrderAtendimento.Infra.MQ/QuickOrderAtendimento.Infra.MQ.csproj", "QuickOrderAtendimento.Infra.MQ/"]
COPY ["QuickOrderAtendimento.IoC/QuickOrderAtendimento.IoC.csproj", "QuickOrderAtendimento.IoC/"]
COPY ["QuickOrderAtendimento.Tests/QuickOrderAtendimento.Tests.csproj", "QuickOrderAtendimento.Tests/"]

RUN dotnet restore "./QuickOrderAtendimento.Api/./QuickOrderAtendimento.Api.csproj"
COPY . .
WORKDIR "/src/QuickOrderAtendimento.Api"
RUN dotnet build "./QuickOrderAtendimento.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QuickOrderAtendimento.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuickOrderAtendimento.Api.dll"]
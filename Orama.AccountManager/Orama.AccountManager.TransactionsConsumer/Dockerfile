#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Orama.AccountManager.TransactionsConsumer/Orama.AccountManager.TransactionsConsumer.csproj", "Orama.AccountManager.TransactionsConsumer/"]
COPY ["Orama.AccountManager.Application/Orama.AccountManager.Application.csproj", "Orama.AccountManager.Application/"]
COPY ["Orama.AccountManager.CrossCutting/Orama.AccountManager.CrossCutting.csproj", "Orama.AccountManager.CrossCutting/"]
COPY ["Orama.AccountManager.Infrastructure/Orama.AccountManager.Infrastructure.csproj", "Orama.AccountManager.Infrastructure/"]
COPY ["Orama.AccountManager.Model/Orama.AccountManager.Model.csproj", "Orama.AccountManager.Model/"]
RUN dotnet restore "Orama.AccountManager.TransactionsConsumer/Orama.AccountManager.TransactionsConsumer.csproj"
COPY . .
WORKDIR "/src/Orama.AccountManager.TransactionsConsumer"
RUN dotnet build "Orama.AccountManager.TransactionsConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orama.AccountManager.TransactionsConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orama.AccountManager.TransactionsConsumer.dll"]
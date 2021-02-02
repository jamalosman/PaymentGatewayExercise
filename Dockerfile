FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80 443 1433 1434 65299 5672

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["./PaymentGateway.Api/PaymentGateway.Api.csproj", "./PaymentGateway.Api/"]
COPY ["./PaymentGateway.Services/PaymentGateway.Services.csproj", "./PaymentGateway.Services/"]
COPY ["./PaymentGateway.Domain/PaymentGateway.Domain.csproj", "./PaymentGateway.Domain/"]
COPY ["./PaymentGateway.Data/PaymentGateway.Data.csproj", "./PaymentGateway.Data/"]
COPY ["./PaymentGateway.Messages/PaymentGateway.Messages.csproj", "./PaymentGateway.Messages/"]
RUN dotnet restore "./PaymentGateway.Api/PaymentGateway.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./PaymentGateway.Api/PaymentGateway.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "./PaymentGateway.Api/PaymentGateway.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaymentGateway.Api.dll"]

FROM microsoft/dotnet:3.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5001

FROM microsoft/dotnet:3.1-sdk AS build
WORKDIR /src
COPY ["PaymentGateway.Api.csproj", "./"]
RUN dotnet restore "./PaymentGateway.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PaymentGateway.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PaymentGateway.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaymentGateway.Api.dll"]

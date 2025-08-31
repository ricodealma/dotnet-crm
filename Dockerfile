FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Dotnet.Crm.App/Dotnet.Crm.App.csproj", "src/Dotnet.Crm.App/"]
COPY ["src/Dotnet.Crm.Domain/Dotnet.Crm.Domain.csproj", "src/Dotnet.Crm.Domain/"]
COPY ["src/Dotnet.Crm.Infra/Dotnet.Crm.Infra.csproj", "src/Dotnet.Crm.Infra/"]


RUN dotnet restore "src/Dotnet.Crm.App/Dotnet.Crm.App.csproj"
COPY . .
WORKDIR "/src/src/Dotnet.Crm.App"
RUN dotnet build "Dotnet.Crm.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dotnet.Crm.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
ENTRYPOINT ["dotnet", "Dotnet.Crm.App.dll"]
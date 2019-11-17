FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["iwor.api/iwor.api.csproj", "iwor.api/"]
COPY ["iwor.DAL/iwor.DAL.csproj", "iwor.DAL/"]
COPY ["iwor.core/iwor.core.csproj", "iwor.core/"]
RUN dotnet restore "iwor.api/iwor.api.csproj"
COPY . .
WORKDIR "/src/iwor.api"
RUN dotnet build "iwor.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iwor.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iwor.api.dll"]
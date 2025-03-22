FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/backend/src/Services/Member/Member.BackEnd.API/Member.BackEnd.API.csproj", "src/backend/src/Services/Member/Member.BackEnd.API/"]
RUN dotnet restore "src/backend/src/Services/Member/Member.BackEnd.API/Member.BackEnd.API.csproj"
COPY . .
WORKDIR "/src/src/backend/src/Services/Member/Member.BackEnd.API"
RUN dotnet build "Member.BackEnd.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Member.BackEnd.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Member.BackEnd.API.dll"]

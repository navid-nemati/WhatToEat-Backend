FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY Authentication_Practice.csproj ./

RUN dotnet restore Authentication_Practice.csproj

COPY . .

RUN dotnet publish Authentication_Practice.csproj -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Authentication_Practice.dll"]
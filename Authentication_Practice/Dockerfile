FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# کپی کردن csproj و restore کردن پکیج‌ها (برای cache بهتر)
COPY Authentication_Practice.csproj ./
RUN dotnet restore Authentication_Practice.csproj

# کپی کل سورس کد
COPY . .

# publish کردن پروژه
RUN dotnet publish Authentication_Practice.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---- مرحله اجرا ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# تنظیم URL روی 0.0.0.0 و استفاده از پورت Render
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet Authentication_Practice.dll --urls=http://0.0.0.0:${PORT:-8080}"]
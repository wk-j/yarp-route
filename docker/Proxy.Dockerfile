FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /app

COPY src/Proxy/*.csproj src/Proxy/Proxy.csproj
RUN  dotnet restore src/Proxy

COPY src/Proxy src/Proxy
RUN dotnet publish src/Proxy -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app

ENV TZ='Asia/Bangkok'
RUN apk add --no-cache tzdata

COPY --from=build /app/out ./

ENTRYPOINT [ "dotnet", "Proxy.dll" ]
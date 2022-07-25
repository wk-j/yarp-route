FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /app

COPY src/MyAdmin/*.csproj src/MyAdmin/MyAdmin.csproj
RUN  dotnet restore src/MyAdmin

COPY src/MyAdmin src/MyAdmin
RUN dotnet publish src/MyAdmin -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app

ENV TZ='Asia/Bangkok'
RUN apk add --no-cache tzdata

COPY --from=build /app/out ./

ENTRYPOINT [ "dotnet", "MyAdmin.dll" , "--urls=http://*:8020" ]
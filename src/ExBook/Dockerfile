#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/ExBook/ExBook.csproj", "src/ExBook/"]
RUN dotnet restore "src/ExBook/ExBook.csproj"
COPY . .
WORKDIR "/src/src/ExBook"
RUN dotnet build "ExBook.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExBook.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExBook.dll"]
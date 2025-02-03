FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["MainProgram/MainProgram.csproj", "MainProgram/"]
RUN dotnet restore "MainProgram/MainProgram.csproj"

COPY . .
WORKDIR "/src/MainProgram"
RUN dotnet publish "MainProgram.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MainProgram.dll"]
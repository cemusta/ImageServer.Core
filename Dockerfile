FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app/image-server

# copy everything and build app
COPY ./ImageServer.Core .
RUN dotnet publish -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY --from=build /app/image-server/out ./app
WORKDIR /app
ENTRYPOINT ["dotnet", "ImageServer.Core.dll"]
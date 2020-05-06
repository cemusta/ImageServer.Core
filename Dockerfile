FROM mcr.microsoft.com/dotnet/core/sdk:3.1.201-buster AS build
WORKDIR /app/image-server

# copy everything and build app
COPY ./ImageServer.Core .
RUN dotnet publish -c Release -r linux-x64 -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1.3-buster-slim  as final
COPY --from=build /app/image-server/out ./app
WORKDIR /app
ENTRYPOINT ["./ImageServer.Core"]
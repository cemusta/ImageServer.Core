FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app/image-server

# copy everything and build app
COPY ./ImageServer.Core .
RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/dotnet:2.2-runtime
COPY --from=build /app/image-server/out ./app
WORKDIR /app
ENTRYPOINT ["dotnet", "ImageServer.Core.dll"]
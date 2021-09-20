FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/EditRelease/EditRelease.csproj", "EditRelease/"]
RUN dotnet restore EditRelease/EditRelease.csproj
COPY ["src/EditRelease", "EditRelease/"]
RUN dotnet build EditRelease/EditRelease.csproj -c Release --no-restore -o /app/build
RUN dotnet publish EditRelease/EditRelease.csproj -c Release --no-restore -o /app/publish

# Label the container
LABEL maintainer="Irongut <murray.dave@outlook.com>"
LABEL repository="https://github.com/irongut/EditRelease"
LABEL homepage="https://github.com/irongut/EditRelease"

# Label as GitHub Action
LABEL com.github.actions.name="Edit Release"
LABEL com.github.actions.description="A GitHub Action for editing release details."
LABEL com.github.actions.icon="edit"
LABEL com.github.actions.color="purple"

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "/app/EditRelease.dll"]
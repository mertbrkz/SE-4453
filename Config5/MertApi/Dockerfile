FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Config5/MertApi/MertApi.csproj", "Config5/MertApi/"]
RUN dotnet restore "Config5/MertApi/MertApi.csproj"
COPY . .
WORKDIR "/src/Config5/MertApi"
RUN dotnet build "MertApi.csproj" -c Release -o /app/build
RUN dotnet publish "MertApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 2222

# Install OpenSSH and configure
RUN apt-get update \
    && apt-get install -y --no-install-recommends openssh-server \
    && echo "root:Docker!" | chpasswd \
    && chmod 755 /app

COPY Config5/MertApi/sshd_config /etc/ssh/

COPY --from=build /app/publish .

COPY Config5/MertApi/entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

ENTRYPOINT ["/app/entrypoint.sh"]

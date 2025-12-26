FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MertApi.csproj", "./"]
RUN dotnet restore "MertApi.csproj"
COPY . .
WORKDIR "/src/."
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

COPY sshd_config /etc/ssh/

COPY --from=build /app/publish .

COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

ENTRYPOINT ["/app/entrypoint.sh"]

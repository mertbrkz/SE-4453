#!/bin/sh
set -e

# Start SSH (Azure App Service requirement)
mkdir -p /run/sshd
/usr/sbin/sshd

# Start App
echo "Starting Application..."
dotnet MertApi.dll
#!/bin/sh
set -e

# Start SSH service
service ssh start

# Start the application
dotnet MertApi.dll

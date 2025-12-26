#!/bin/sh
set -e

# Start SSH
mkdir -p /run/sshd
/usr/sbin/sshd

# Start App
dotnet MertApi.dll
#!/bin/bash

# Wait for the database to be ready
sleep 5

# Apply migrations
dotnet ef database update --project AuthenticationAPI.csproj

# Start the application
dotnet AuthenticationAPI.dll 
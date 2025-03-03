#!/bin/bash

# Wait for the database to be ready
sleep 5

# Set the environment variable for the database connection
export ConnectionStrings__DefaultConnection="Data Source=/app/data/auth.db"

# Apply migrations
cd /src/AuthenticationAPI
dotnet ef database update --project AuthenticationAPI.csproj

# Start the application
cd /app
dotnet AuthenticationAPI.dll 
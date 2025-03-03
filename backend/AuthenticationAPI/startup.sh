#!/bin/bash

# Wait for the database to be ready
sleep 5

# Set the environment variable for the database connection
export ConnectionStrings__DefaultConnection="Data Source=/app/data/auth.db"

# Create migrations directory if it doesn't exist
mkdir -p /src/AuthenticationAPI/Migrations

# Go to the project directory
cd /src/AuthenticationAPI

# Remove existing migrations if any
rm -f Migrations/*.cs

# Remove existing database
rm -f /app/data/auth.db

# Create new migration
dotnet ef migrations add InitialCreate

# Apply migrations with verbose output
dotnet ef database update --verbose

# Verify database creation
echo "Verifying database tables..."
sqlite3 /app/data/auth.db ".tables"

# Start the application
cd /app
dotnet AuthenticationAPI.dll 
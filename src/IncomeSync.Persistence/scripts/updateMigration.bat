@echo off
cd ../

echo Adding new migration...
dotnet ef migrations add %1

echo Updating the database...
dotnet ef database update

echo Database update complete.

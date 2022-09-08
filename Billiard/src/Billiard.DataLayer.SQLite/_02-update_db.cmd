dotnet tool update --global dotnet-ef --version 6.0.1
dotnet build
dotnet ef --startup-project ../Billiard/ database update --context SQLiteDbContext
pause
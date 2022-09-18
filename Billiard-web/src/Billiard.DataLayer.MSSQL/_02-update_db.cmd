dotnet tool update --global dotnet-ef --version 5.0.0
dotnet build
dotnet ef --startup-project ../Billiard/ database update --context MsSqlDbContext
pause
del .\Migrations\*.cs
dotnet ef database drop -f
dotnet ef migrations add CreateInitial
dotnet ef database update
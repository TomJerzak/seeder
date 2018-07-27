# seeder
Database seeder.

### Add to project
```
<DotNetCliToolReference Include="Seeder" Version="1.0.4" />
```

### Examples
```
dotnet seeder --version
dotnet seeder scripts add ScriptName
dotnet seeder database update "User ID=seeder;Password=seeder;Host=127.0.0.1;Port=5432;Database=seeder;Pooling=true;"
```

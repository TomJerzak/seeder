# seeder

Database seeder for PostgreSQL, SQLite.

## Add to project

```bash
<DotNetCliToolReference Include="Seeder" Version="1.0.12" />
```

## Examples

Seeder can be used from the command line or inside the project.

### Command line

- Extension version

```bash
dotnet seeder --version
```

- Available providers

```bash
dotnet seeder --provider
```

- Create sql script

```bash
dotnet seeder scripts add ScriptName
```

- Database update for PostgreSQL

```bash
dotnet seeder database update --provider PostgreSQL "User ID=seeder;Password=seeder;Host=127.0.0.1;Port=5432;Database=seeder;Pooling=true;"
```

- Database update for SQLite

```bash
dotnet seeder database update --provider SQLite "Data Source=C:\temp\seeder.db;"
```

### Project

```bash
// TODO
```

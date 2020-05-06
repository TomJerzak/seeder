# seeder

Database seeder for PostgreSQL, SQLite.

## Installing the tools

```bash
dotnet tool install --global seeder
```

## Examples

Seeder can be used from the command line or inside the project.

### Command line

- Extension version

```bash
seeder --version
```

- Available providers

```bash
seeder --provider
```

- Create sql script

```bash
seeder scripts add ScriptName
```

- Database update for PostgreSQL

```bash
seeder database update --provider PostgreSQL "User ID=seeder;Password=seeder;Host=127.0.0.1;Port=5432;Database=seeder;Pooling=true;"
```

- Database update for SQLite

```bash
seeder database update --provider SQLite "Data Source=C:\temp\seeder.db;"
```

### Project

```bash
// TODO
```

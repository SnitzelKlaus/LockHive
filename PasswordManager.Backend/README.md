# Template

## Migrations

Create tables for database go to directory "src/KeyVaults/KeyVaults.Infrastructure" to run EF core migraiton commands

```Powershell
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet ef database update --verbose --context SecurityKeyContext --project . --startup-project ../KeyVaults.Api.Service
```


```shell
ASPNETCORE_ENVIRONMENT=Development dotnet ef database update --verbose --context SecurityKeyContext --project . --startup-project ../KeyVaults.Api.Service
```


To add a new migration:

```shell
ASPNETCORE_ENVIRONMENT=Development dotnet ef migrations add AddUser --context SecurityKeyContext --project . --startup-project ../KeyVaults.Api.Service
```

```Powershell
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet ef migrations add AddSecurityKey --context SecurityKeyContext --project . --startup-project ../KeyVaults.Api.Service
```

To remove latest added:
```shell
ASPNETCORE_ENVIRONMENT=Development dotnet ef migrations remove --context SecurityKeyContext --project . --startup-project ../KeyVaults.Api.Service
```

```Powershell
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet ef migrations remove --context SecurityKeyContext --project . --startup-project ../KeyVaults.Api.Service
```

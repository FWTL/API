param(
[Parameter(Mandatory=$false)][string]$buildPath,
[string]$configuration
)

if(!($buildPath)){
    $buildPath = Split-Path -Path $PSScriptRoot -Parent
}
else
{
    $buildPath = Join-Path $buildPath 'FWTL.Api.Database'
}

$Env:ASPNETCORE_ENVIRONMENT = $configuration
Set-Location $buildPath
dotnet ef database update --startup-project ../FWTL.Api --verbose --context UserDatabaseContext

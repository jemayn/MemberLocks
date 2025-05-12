[CmdletBinding()]
param(
    [ValidateScript(
        { $_ -in (Get-ChildItem -Path "$PSScriptRoot\environments\" -Filter "*.env" -File).BaseName }
    )]
    [ArgumentCompleter({
        (Get-ChildItem -Path "$PSScriptRoot\environments\" -Filter "*.env" -File).BaseName
    })]
	[String]$Environment = "local",
    
    [switch]$SkipRunningK6
)

docker-compose build
docker-compose up -d influxdb grafana

if ($SkipRunningK6)
{
    exit
}

Write-Host -ForegroundColor "Blue" -Object "Load testing with Grafana dashboard"
Write-Host -ForegroundColor "Cyan" -Object "- Environment: " -NoNewLine
Write-Host -ForegroundColor "Yellow" -Object $Environment

$envFile = "$PSScriptRoot/environments/$Environment.env"

docker compose --env-file $envFile up k6

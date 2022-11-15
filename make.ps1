[CmdletBinding(PositionalBinding=$false)]
param(
$Configuration = "Release",
[parameter(ValueFromRemainingArguments=$true)]
$Command
)

function Make-All
{
param (
[Parameter()] [string] $Configuration
)		
	if ((ValidateDotnet) -eq 1)
	{
		return
	}

	Write-Host ("Building using '" + $Configuration + "' configuration...") -ForegroundColor Blue
	
	Push-Location
	Set-Location -Path ".\src"
		dotnet build -c $Configuration --nologo -p:TargetPlatform=win-x64
	Pop-Location

	if ($lastexitcode -ne 0)
	{
		Write-Host "Make all failed. Check logs for details." -ForegroundColor Red
	}
	else
	{
		Write-Host "Make all succeeded." -ForegroundColor Green
	}
}

function Make-Clean
{
	if ((ValidateDotnet) -eq 1)
	{
		return
	}

	Write-Host ("Cleaning...") -ForegroundColor Blue

	Push-Location
	Set-Location -Path ".\src"
		dotnet clean /nologo
	Pop-Location
	
	Write-Host "Clean finished." -ForegroundColor Green
}

function ValidateDotnet
{
	if ((Get-Command "dotnet" -ErrorAction SilentlyContinue) -eq $null)
	{
		Write-Host "The 'dotnet' tool is not available. Please install .NET 6.0 or higher. More info at https://dotnet.microsoft.com/download" -ForegroundColor Red
		return 1
	}

	return 0
}

switch ($Command)
{
	"all"   { Make-All -Configuration $Configuration }
	"clean" { Make-Clean }
	Default { Write-Host ("Invalid command '{0}'" -f $Command) }
}
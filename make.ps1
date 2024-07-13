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

	$slnFile = "OpenBreed.sln"

	Write-Host ("Building '" + $slnFile + "' using '" + $Configuration + "' configuration...") -ForegroundColor Blue
	
	Push-Location
	
	try
	{
		Set-Location -Path ".\src"
		
	
		dotnet build $slnFile -c $Configuration --nologo -p:TargetPlatform=win-x64
		
		if ($lastexitcode -ne 0)
		{
			Write-Host "dotnet build failed." -ForegroundColor Red
			return
		}	
		
		dotnet publish "OpenBreed.Sandbox\OpenBreed.Sandbox.csproj" -c $Configuration -r win-x64 -o ".\..\build"
		
		if ($lastexitcode -ne 0)
		{
			Write-Host "dotnet publish failed." -ForegroundColor Red
			return
		}
	}
	finally
	{
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
}

function Make-Clean
{		
	
	if ((ValidateDotnet) -eq 1)
	{
		return
	}

	$slnFile = "OpenBreed.sln"

	Write-Host ("Cleaning '" + $slnFile + "'...") -ForegroundColor Blue

	Push-Location
	
	try
	{
		Set-Location -Path ".\src"
		
		dotnet clean $slnFile /nologo
	}
	finally
	{
		Pop-Location
	}
	
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
# Specify parameters and shortcuts
# Example values, see secrets 
param(
    [Alias("sa")][string]$ServerAddress = "localhost",
    [Alias("un")][string]$UserName = "BerghAdmin",
    [Alias("db")][string]$DatabaseName = "BIHZ2021",
    [Alias("sf")][string]$SourceFilename ="c:\tmp\MySQLBackup.sql"
)

# Ensure the parameters are provided
if (-not $ServerAddress -or -not $UserName -or -not $DatabaseName -or -not $SourceFilename) {
    Write-Host "Usage: .\RestoreDb.ps1 -ServerAddress (sa) <server> -UserName (un) <username> -DatabaseName (db) <database> -SourceFilename (sf) <filename>"
    exit 1
}
# Check if mysqldump is available in the specified path
$mysqlrestoreCommand = "C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe"
if (-not (Test-Path $mysqlrestoreCommand)) {
    Write-Host "mysql utility not found at $mysqlrestoreCommand"
    exit 1
}

# Ensure the source file exists
if (-not (Test-Path $SourceFilename)) {
    Write-Host "Source file not found: $SourceFilename"
    exit 1
}

$mysqlrestoreArgs = @(
    "--host", $ServerAddress,
    "--user", $UserName,
    "--default-character-set=utf8mb4",  # Use UTF-8 encoding
    "-p", 
    $DatabaseName
)
# Execute the mysqldump command
$ErrorActionPreference = "Stop"  # Stop on any error
try {
    Write-Host "Restoring database backup, command: Get-Content $SourceFilename | $mysqlrestoreCommand $($mysqlrestoreArgs -join ' ')"
    Get-Content $SourceFilename | & $mysqlrestoreCommand $mysqlrestoreArgs
    Write-Host "Database restore completed successfully: $SourceFilename"
} catch {
    Write-Host "An error occurred while restoring the database: $_"
    exit 1
}

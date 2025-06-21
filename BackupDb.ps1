# Specify parameters and shortcuts
# Example values, see secrets 
param(
    [Alias("sa")][string]$ServerAddress = "localhost",
    [Alias("un")][string]$UserName = "BerghAdmin",
    [Alias("db")][string]$DatabaseName = "BIHZ2021",
    [Alias("tf")][string]$TargetFilename ="c:\tmp\MySQLBackup.sql"
)

# Ensure the parameters are provided
if (-not $ServerAddress -or -not $UserName -or -not $DatabaseName -or -not $TargetFilename) {
    Write-Host "Usage: .\BackupDb.ps1 -ServerAddress (sa) <server> -UserName (un) <username> -DatabaseName (db) <database> -TargetFilename (tf) <filename>"
    exit 1
}
# Check if mysqldump is available in the specified path
$mysqldumpCommand = "C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe"
if (-not (Test-Path $mysqldumpCommand)) {
    Write-Host "mysqldump utility not found at $mysqldumpCommand"
    exit 1
}
# Ensure the target directory exists
$targetDirectory = Split-Path -Path $TargetFilename -Parent
if (-not (Test-Path $targetDirectory)) {
    Write-Host "Target directory does not exist: $targetDirectory"
    exit 1
}
# Create the target directory if it does not exist
New-Item -ItemType Directory -Path $targetDirectory -Force | Out-Null
# Construct the command to execute mysqldump with the provided parameters
$mysqldumpArgs = @(
    "--host", $ServerAddress,
    "--user", $UserName,
    "--default-character-set=utf8mb4",  # Use UTF-8 encoding
    "--no-tablespaces",  # Exclude tablespaces
    "--skip-lock-tables",  # Skip locking tables
    "--single-transaction",  # dump in a single transaction
    "-p", 
    $DatabaseName
)
# Execute the mysqldump command
$ErrorActionPreference = "Stop"  # Stop on any error
try {
    Write-Host "Starting database backup, command: $mysqldumpCommand $($mysqldumpArgs -join ' ') > $TargetFilename"
    & $mysqldumpCommand $mysqldumpArgs > $TargetFilename
    Write-Host "Database backup completed successfully: $TargetFilename"
} catch {
    Write-Host "An error occurred while backing up the database: $_"
    exit 1
}

#
# File will create all secrets in the local user story
# Make sure the file is in the root directory of the solution
#
dotnet user-secrets clear --project ./BerghAdmin
Write-Host "Cleared all BergAdmin secrets" -ForegroundColor Green
type .\secrets.json | dotnet user-secrets set --project "BerghAdmin"
Write-Host "Added all BergAdmin secrets"  -ForegroundColor Green

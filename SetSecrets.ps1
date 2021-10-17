#
# File will create all secrets in the local user story
# Make sure the file is in the root directory of the solution
#
type .\secrets.json | dotnet user-secrets set --project "BerghAdmin"

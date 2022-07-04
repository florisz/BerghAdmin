param($env = "test")

if ($env -ne "test" -and $env -ne "prod")
{
    Write-Error 'Parameter env can only be: test or prod'
    throw 'Parameter errorenv must prod or test'
}


$ti= (Get-Culture).TextInfo
$envUpper = $ti.ToTitleCase($env)
$rg = "Bergh$envUpper"
$location = "westeurope"

$plan = "bergh-$env-plan"
$webapp = "bergh-$env-admin-webapp"
$webmonitor = "bergh-$env-monitor-webapp"
$storageaccount = "bergh$($env)storage"
$workspace = "bergh-$env-workspace"
$appinsights = "bergh-$env-appinsights"
$functionplan = "bergh-$env-functionplan"
$functionappkentaa = "bergh-$env-kentaa-functionapp"
$keyvault = "bergh-$env-keyvault"
$webappsettings = @(
  "VaultName=bergh-$env-keyvault",
  "ASPNETCORE_ENVIRONMENT=$env"
)

# Test prequisites
#   - database should exist
#   - keyvault should exist
# TO BE DONE

# setup environment clean up the current resource group and create a new one
write-host "Delete azure group $rg in $location" -ForegroundColor yellow
az group delete `
    --name $rg `
    --yes

write-host "Create azure group $rg in $location" -ForegroundColor yellow
az group create `
    --name $rg `
    --location $location

# write-host "Create azure keyvault $keyvault (in $rg at $location)" -ForegroundColor yellow
# az keyvault create `
#     --resource-group $rg `
#     --name $keyvault `
#     --location $location `
#     --sku Standard

#### Web Admin App
write-host "Create azure appservice plan $plan (in $rg at $location)" -ForegroundColor yellow
az appservice plan create `
    --resource-group $rg `
    --name $plan `
    --location $location `
    --sku B1 `
    --is-linux `
    --number-of-workers 1

write-host "Create azure webapp $webapp (in $rg and $plan)" -ForegroundColor yellow
$webappId = az webapp create `
    --name $webapp `
    --resource-group $rg `
    --plan $plan `
    --runtime '"dotnetcore|6.0"' 

write-host "Create azure webapp config settings for $webapp set keyvault to $keyvault" -ForegroundColor yellow
$webappId = az webapp config appsettings set `
    --resource-group $rg `
    --name $webapp `
    --settings @webappsettings

write-host "Create azure webapp managed identity for $webapp " -ForegroundColor yellow
az webapp identity assign `
    --resource-group $rg `
    --name $webapp 

write-host "Set azure keyvault permissions for $webapp" -ForegroundColor yellow
az keyvault set-policy `
    --secret-permissions get list `
    --name $keyvault `
    --object-id $webappId

### Web Monitor app
write-host "Create azure webapp $webmonitor (in $rg and $plan)" -ForegroundColor yellow
$monitorId = az webapp create `
    --name $webmonitor `
    --resource-group $rg `
    --plan $plan `
    --runtime '"dotnetcore|6.0"' `
    --assign-identity [system] `
    --query identity.principalId

write-host "Create azure webapp config settings for $webmonitor set keyvault to $keyvault" -ForegroundColor yellow
az webapp config appsettings set `
    --resource-group $rg `
    --name $webmonitor `
    --settings @webappsettings

write-host "Set keyvault permissions for $webmonitor" -ForegroundColor yellow
az keyvault set-policy `
    --secret-permissions get list `
    --name $keyvault `
    --object-id $monitorId

#### Function Apps
write-host "Create azure storage account $storageaccount (in $rg at $location)" -ForegroundColor yellow
az storage account create `
    --name $storageaccount `
    --resource-group $rg `
    --location $location `
    --sku Standard_LRS	

write-host "Enable dynamic installation of extensions" 
az config set extension.use_dynamic_install=yes_without_prompt

write-host "Create azure monitor log-analytics workspace $workspace  (in $rg at $location)" -ForegroundColor yellow
az monitor log-analytics workspace create `
    --workspace-name $workspace `
    --resource-group $rg `
    --location $location `
    --retention-time 30

write-host "Create azure monitor app-insights component app $appinsights  (in $rg at $location)" -ForegroundColor yellow
az monitor app-insights component create `
    --resource-group $rg `
    --location $location `
    --app $appinsights

write-host "Create azure functionapp plan $functionplan (in $rg at $location)" -ForegroundColor yellow
az functionapp plan create `
    --name $functionplan `
    --resource-group $rg `
    --location $location `
    --sku B1 `
    --is-linux `
    --number-of-workers 1

write-host "Create azure functionapp $functionappkentaa (in $rg and $functionplan)" -ForegroundColor yellow
$functionAppId = az functionapp create `
    --name $functionappkentaa `
    --resource-group $rg `
    --os-type Linux `
    --functions-version 4 `
    --runtime dotnet `
    --runtime-version 6 `
    --storage-account $storageaccount `
    --plan $functionplan `
    --assign-identity [system] `
    --query identity.principalId

write-host "Create azure functionapp config $functionappkentaa in $rg" -ForegroundColor yellow
az functionapp config appsettings set `
    --name $functionappkentaa `
    --resource-group $rg `
    --settings "AZURE_FUNCTIONS_ENVIRONMENT=$env" "cron_users=0 0 1 * * *" "cron_projects=0 0 2 * * *" "cron_actions=0 0 3 * * *" "cron_donations=0 0 4 * * *"

write-host "Set keyvault permissions for $functionappkentaa" -ForegroundColor yellow
az keyvault set-policy `
    --secret-permissions get list `
    --name $keyvault `
    --object-id $functionAppId

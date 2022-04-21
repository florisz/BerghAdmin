$rg = "BerghTest"
$location = "westeurope"

$plan = "bergh-test-plan"
$webapp = "bergh-test-bergh-admin-webapp"
$webmonitor = "bergh-test-bergh-monitor-webapp"
$storageaccount = "bergh-test-storage-account"
$workspace = "bergh-test-workspace"
$appinsights = "bergh-test-appinsights"
$functionplan = "bergh-test-functionplan"
$functionappkentaa = "bergh-test-functionapp-kentaa"

az group create `
    --name $rg `
    --location $location

az appservice plan create `
    --resource-group $rg `
    --name $plan `
    --location $location `
    --sku B1 `
    --is-linux `
    --number-of-workers 1

#### Web Apps
az webapp create `
    --name $webapp `
    --resource-group $rg `
    --plan $plan `
    --runtime '"dotnetcore|6.0"' 

az webapp create `
    --name $webmonitor `
    --resource-group $rg `
    --plan $plan `
    --runtime '"dotnetcore|6.0"' 

#### Function Apps
az storage account create `
    --name $storageaccount `
    --resource-group $rg `
    --location $location `
    --sku Standard_LRS	

az monitor log-analytics workspace create `
    --workspace-name $workspace `
    --resource-group $rg `
    --location $location `
    --retention-time 30

az monitor app-insights component create `
    --resource-group $rg `
    --location $location `
    --app $appinsights
    # --app-id $appid

az functionapp plan create `
    --name $functionplan `
    --resource-group $rg `
    --location $location `
    --sku B1 `
    --is-linux `
    --number-of-workers 1

az functionapp create `
    --name $functionappkentaa `
    --resource-group $rg `
    --os-type Linux `
    --functions-version 4 `
    --runtime dotnet `
    --runtime-version 6 `
    --storage-account $storageaccount `
    --plan bergh-uat-functions

az functionapp config appsettings set `
    --name $functionappkentaa `
    --resource-group $rg `
    --settings "?????????????AzureWebJobsStorage=$storageConnectionString"
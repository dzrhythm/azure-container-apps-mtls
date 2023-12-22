# Commands for setting up required resources in Azure running the demo.
# Consider running one command at a time to follow along;
# In Visual Studio Code with the PowerShell Extension you can execute a line
# by placing the cursor on it and pressing the F8 key,
# or run multiple lines by selecting them and pressing the F8 key.

# Update and set these variable values before continuing
$resourceGroup='container-apps'
$acrName='aisdzcontainerappscr' # Must be unique amongst all Azure container registries
$location='EastUS'
$appEnvironment='widget-app-env-mtls'
$imageNameApp='widgetorderapp-mtls'
$imageNameSvc='widgetordersvc-mtls'
$containerPort='5004'
$imageTag='1.0.0'

# One-time: create a local user-defined network to use for local debugging in containers
docker network create widget-net

# Login to Azure
az login

# Add/Update the containerapp extension to Azure CLI
az extension add --name containerapp --upgrade
az provider register --namespace Microsoft.App
az provider register --namespace Microsoft.OperationalInsights

# Create the resource group
az group create --name $resourceGroup --location $location

# Create the ACR
az acr create --resource-group $resourceGroup --name $acrName --sku Basic --admin-enabled true

# You may need to manually assign an ACR role to your account:
# 1. Go to the Azure Container Registry in the Azure Portal.
# 2. Select Access Control (IAM).
# 3. Click on "Add role assignment" under "Grant access to this resouce"
# 4. Now go to "Priviledged administrator roles" and select "Owner"
# 5. Click on "Next" at the bottom of the page.
# 6. Now click on "Select Members"
# 7. Add yourself (via email or name) and click on "Select" at the bottom of the right expanded pane.
# 8. Select "Not constrained" for Delegation type.
# 9. Click on "Review and Assign"

# Login to the ACR
az acr login --name $acrName


# Create the container apps environment with mTLS enabled
az containerapp env create `
  --name $appEnvironment `
  --resource-group $resourceGroup `
  --location $location `
  --enable-mtls

##############################################################################
#
# Service container app
#

# Build and push the service and app images
docker build --rm --pull -f src/WidgetOrderService/Dockerfile -t "${imageNameSvc}:$imageTag" src/WidgetOrderService
docker tag "${imageNameSvc}:$imageTag" "$acrName.azurecr.io/${imageNameSvc}:$imageTag"
docker push "$acrName.azurecr.io/${imageNameSvc}:$imageTag"

# Create or update the container app
az containerapp create `
  --name $imageNameSvc `
  --resource-group $resourceGroup `
  --environment $appEnvironment `
  --system-assigned `
  --registry-server "$acrName.azurecr.io" `
  --registry-identity system `
  --ingress external `
  --target-port $containerPort

# Grant the container app access to the ACR
az containerapp registry set --name $imageNameSvc --resource-group $resourceGroup --identity system --server "$acrName.azurecr.io"

# Deploy our container image
az containerapp update --name $imageNameSvc --resource-group $resourceGroup --image "$acrName.azurecr.io/${imageNameSvc}:$imageTag"

##############################################################################
#
# UI container app
#

# Build and push the service and app images
docker build --rm --pull -f src/WidgetOrderApp/Dockerfile -t "${imageNameApp}:$imageTag" src/WidgetOrderApp
docker tag "${imageNameApp}:$imageTag" "$acrName.azurecr.io/${imageNameApp}:$imageTag"
docker push "$acrName.azurecr.io/${imageNameApp}:$imageTag"

# Create or update the container app
az containerapp create `
  --name $imageNameApp `
  --resource-group $resourceGroup `
  --environment $appEnvironment `
  --system-assigned `
  --registry-server "$acrName.azurecr.io" `
  --registry-identity system `
  --ingress external `
  --target-port $containerPort

# Grant the container app access to the ACR
az containerapp registry set --name $imageNameApp --resource-group $resourceGroup --identity system --server "$acrName.azurecr.io"

# Deploy our container image
az containerapp update --name $imageNameApp --resource-group $resourceGroup --image "$acrName.azurecr.io/${imageNameApp}:$imageTag"

# Go to the Azure Portal to view your azure container apps. In the Overview blade you will find the URL to
# connect to your applications.

# Cleanup when ready
#az group delete --name $resourceGroup --yes

# Azure Container Apps With mTLS Demo

Demo repo for deploying containerized services to Azure Container Apps with the mTLS option enabled
in the Azure Container App Environment. It includes:

- A simple order app (UI).
- A simple order service, called by the order app.
- Setup for debugging each service in Docker using Visual Studio Code.
- A solution file and Docker compose project for debugging in Visual Studio.
- A [setup.ps1](./setup.ps1) with commands for:
  - Provisioning resources in Azure.
  - Building and pushing the container images.
  - Creating and deploying the Azure Container Apps.

The applications are written using .NET and C#.

## Prerequisites

- A Docker daemon/CLI environment (e.g. [Docker Desktop](https://www.docker.com/products/docker-desktop/) or [Rancher Desktop](https://rancherdesktop.io/)).
- [Visual Studio Code](https://code.visualstudio.com/Download) or [Visual Studio](https://visualstudio.microsoft.com/downloads/).
- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- The [Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli)

You should be generally familiar with Azure and building application container images before using this repo.

## Visual Studio Code

To debug locally using containers in Visual Studio Code:

1. Create the shared local Docker bridge network by running: `docker network create widget-net`
2. Make sure you have the [Docker Extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-docker) for VS Code installed.
3. Open each project folder in a different Visual Studio Code instance (src/WidgetOrderApp and src/WidgetOrderService).
4. On the debug tab, select the "Docker .NET Launch" profile.
5. Start debugging on each service.

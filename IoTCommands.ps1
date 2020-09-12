# Install Azure CLI - https://aka.ms/installazurecliwindows

# pre-requisites 
az --version 
az extension show --name azure-iot
az extension add --name azure-iot
az extension update --name azure-iot
az extension list 

az login 
az account list --output table 
az account set --subscription $subscriptionId
az account show 


# parameters 
$subscriptionId = '<subscription-id>'
$resourceGroup = '<resource-group>'
$location = '<location>'
$iotHubName = '<iot-hub-name>'
$deviceId = '<device-id>'
$dpsName = '<dps-name'

az iot hub list 
az group create --name $resourceGroup --location $location
az iot hub create --name $iotHubName --resource-group $resourceGroup

az iot hub show --name $iotHubName
az iot hub device-identity list --name $iotHubName --output table
az iot hub device-identity create --name $iotHubName --device-id $deviceId
az iot hub device-identity show --name $iotHubName --device-id $deviceId
az iot hub device-twin show -d $deviceId -n $iotHubName

# send device to cloud messages 
az iot hub monitor-events -d $deviceId -n $iotHubName
az iot device send-d2c-message -n $iotHubName -d $deviceId --data 'ping from the terminal'

# send cloud to device messages 
az iot device c2d-message send -n $iotHubName -d $deviceId --data 'hello world'
az iot device c2d-message receive -n $iotHubName -d $deviceId --data 'hello world'

# simulate a device 
az iot hub monitor-events -n $iotHubName -d $deviceId 
az iot device simulate -n $iotHubName -d $deviceId --data "Message from simulated device" --msg-count 10 --msg-interval 5

# update device-twin properties
az iot hub device-twin show -d $deviceId -n $iotHubName
az iot hub device-twin update -d $deviceId -n $iotHubName --set tags="{'location':{'region':'US'}}"
az iot hub device-twin update -d $deviceId -n $iotHubName --set tags.location.region='null'
az iot hub device-twin update -d $deviceId -n $iotHubName --set properties.desired.interval = 3000


az iot dps show --name $dpsName -g $resourceGroup
az iot dps create --name $dpsName --resource-group $resourceGroup 
az iot dps linked-hub list --dps-name $dpsName --output table
$iotHubConnectionString = az iot hub show-connection-string --name $iotHubName
az iot dps linked-hub create --dps-name $dpsName -g $resourceGroup --connection-string $iotHubConnectionString --location $location
az iot dps linked-hub show --dps-name $dpsName -g $resourceGroup --linked-hub $iotHubName












# Serverless and DB
In this exercise, we are going to learn about Azure Functions, SignalR service, Event Hub, IoT Hub custom routing and Azure Storage Table. This will allow us to connect our device to our mobile app and save the data between sessions.

## Create custom routing for IoT Hub device-to-cloud messages
Read about creating a custom routing for device-to-cloud messages: 
https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-read-custom

Follow these instructions to create a new Event Hub:
https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-create

Create custom routing to the new Event Hub in the IoT Hub you’ve created in your previous assignment. 

## Create azure storage table
Create a storage table to save the current counter across the system.
https://docs.microsoft.com/en-us/azure/storage/tables/table-storage-quickstart-portal

Create a table and initialize it with a 0 value.
We will now proceed to save the current counter from different sources.

## Create Azure Functions
Read about how you can create azure functions here:
https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-your-first-function-visual-studio

We will create 3 different azure functions. Make sure that after you test those functions, you publish them on your Azure account.

### MessageReceiver
Create a new azure function to receive messages from your custom Event Hub.
Choose the appropriate trigger and make sure that pressing the button on your IoT device from the previous assignment actually triggers the function.
Use azure table storage bindings to increment the counter in the storage by 1.

https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-table

### UpdateCounter
Create a new azure function which will be triggered by a regular HTTP call. The purpose of this method is to receive a new counter value and update it in the Azure Storage table.

### GetCounter
Create a new azure function which will be triggered by a regular HTTP call and will return the current counter that is saved in the azure table storage.

## Create SignalR service
Create your own SignalR service in the azure portal. You can follow this section in the Azure Functions integration QuickStart article:
https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-quickstart-azure-functions-csharp#create-an-azure-signalr-service-instance

Read about SignalR azure function bindings:
https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-signalr-service

Update the MessageReceiver and UpdateCounter functions to send a message to everyone with the new counter value.

## Update your Xamarin App
Reopen your Xamarin App from your previous assignment.
When your application starts, connect to the SignalR service and listen to all the incoming messages.
When messages come in, update the counter property with the new value.
https://docs.microsoft.com/en-us/aspnet/core/signalr/dotnet-client?view=aspnetcore-2.2

Also, Call the GetCounter upon startup to receive the initial value of the counter.
When the button is pressed, don’t update the counter property, but only call the function UpdateCounter.

## Test everything
Open your mobile app. Make sure you don’t see 0 as the initial value. 
Press the button in the app. The counter should increase. Make sure that it indeed goes through the azure functions.
Press the button on your IoT device. Make sure that the counter in your mobile app is incremented.

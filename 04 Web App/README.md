# Web App
In this exercise, we are going to create a web app, connect it to our IoT Hub and send messages from it to specific devices, which in turn will increase the counter.

## Create the web app
Create a web app which will:
* On the top of the page, show a label and the current counter.
Make sure that you connect to the SignalR service to receive notifications on the counter change. 
* Show all the connected devices from the IoT Hub.
Check the following documentation for how to query devices from IoT using C#:
https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-query-language#c-example
* For each device, add two “+” buttons. Pressing a button will send a cloud-to-device message to the device with the counter ID, which should trigger the flow of incrementing the relevant counter value. (Same as pressing the physical button).
You can read on how to send these type of messages using C# here:
https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-csharp-csharp-c2d#send-a-cloud-to-device-message

Your web page should look something like this:
![screen shot](./ScreenShot.png)

## Handle the messages on your device
To close the loop, we need to handle the messages that we receive from the cloud on our device. Upon receiving a message, you should start the same flow that the button press on your device starts.
You can see examples of how to receive cloud-to-device messages on your device in this repository:
https://github.com/Azure/azure-iot-sdk-c/tree/master/iothub_client/samples

## Deploy the web app
After you make sure that the device actually receives the messages and handles them appropriately, it is time to deploy your Web App to azure.
You can follow this interactive tutorial to have better understanding on how to deploy a web app to the Azure App Service.
https://docs.microsoft.com/en-us/learn/modules/host-a-web-app-with-azure-app-service/

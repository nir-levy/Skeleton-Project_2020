# IoT Hub and Device SDK
In this exercise, we are going to learn about IoT Hub, the device SDK and how to send data from the device to the cloud when the device user is pressing a button.

## Open your own IoT Hub, register the device and run the sample
Follow these instructions to open your own IoT Hub in the Azure Portal, register a new device and run the sample code:
https://docs.microsoft.com/en-us/azure/iot-hub/quickstart-send-telemetry-c

This will allow you to better understand the use of the device SDK and see a real working sample.

For Arduino devices, there is a port of the C SDK (with included samples). You can find it here:
https://github.com/Azure/azure-iot-arduino

## Device logic
Attach a button to your IoT device. The device should send a message to the IoT Hub every time the user is pressing the button.
You can find a sample of a working button loop in this GitHub repo: https://github.com/flaviomauro/Adafruit_Feather_Led_Button

Combine the Arduino SDK and the code above to send a message to the IoT Hub each time the button is pressed.

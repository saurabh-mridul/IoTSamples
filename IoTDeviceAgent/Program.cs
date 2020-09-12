using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace IoTDeviceAgent
{
    class Program
    {
        private static DeviceClient _device;
        private const string DeviceConnectionString = "<Enter your device connection string>";

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Initializing Device Agent ...");

                _device = DeviceClient.CreateFromConnectionString(DeviceConnectionString);
                await _device.OpenAsync();

                Console.WriteLine("Device is connected to Azure IoT Hub.");

                await SendMessagesAsync(_device);
                //await UpdateDigitalTwinAsync(_device);

                //await SubscribeToEventsAsync(_device);
                //await _device.SetMethodDefaultHandlerAsync(OtherDeviceMethodAsync, null);
                //await _device.SetMethodHandlerAsync("showMessage", ShowMessageAsync, null);

                Console.WriteLine("Press any key to exit ...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task SendMessagesAsync(DeviceClient device)
        {
            var count = 1;
            while (true)
            {
                var telemetry = new Telemetry
                {
                    Message = "Sending complex object to the cloud",
                    StatusCode = count++
                };

                var telemetryJsonString = JsonConvert.SerializeObject(telemetry);

                var message = new Message(Encoding.ASCII.GetBytes(telemetryJsonString));
                await device.SendEventAsync(message);

                Console.WriteLine("message sent to the cloud.");
                await Task.Delay(2000);
            }
        }

        private static async Task UpdateDigitalTwinAsync(DeviceClient device)
        {
            var twinProperties = new TwinCollection();
            twinProperties["connectionType"] = "wi-fi";
            twinProperties["connectionStrength"] = "weak";

            await device.UpdateReportedPropertiesAsync(twinProperties);
            Console.WriteLine("Twin properties updated successfully.");
        }

        private static async Task SubscribeToEventsAsync(DeviceClient device)
        {
            Console.WriteLine("subscribed to events...");
            while (true)
            {
                var message = await device.ReceiveAsync();
                if (message == null)
                    continue;

                var messageBody = message.GetBytes();
                var payload = Encoding.ASCII.GetString(messageBody);
                Console.WriteLine($"Received message from the cloud: {payload}");
                await device.CompleteAsync(message);
            }
        }

        private static Task<MethodResponse> ShowMessageAsync(MethodRequest request, object userContext)
        {
            Console.WriteLine("***Message Received***");
            Console.WriteLine(request.DataAsJson);

            var responsePayLoad = Encoding.ASCII.GetBytes("{\"response\": \"Message Shown!\" ");

            return Task.FromResult(new MethodResponse(responsePayLoad, 200));
        }

        private static Task<MethodResponse> OtherDeviceMethodAsync(MethodRequest request, object userContext)
        {
            Console.WriteLine("***Other device method called***");
            Console.WriteLine($"Method: {request.Name}");
            Console.WriteLine($"Payload: {request.DataAsJson}");

            var responsePayLoad = Encoding.ASCII.GetBytes("{\"response\": \"This method is not implemented.\" ");

            return Task.FromResult(new MethodResponse(responsePayLoad, 404));
        }

    }
}

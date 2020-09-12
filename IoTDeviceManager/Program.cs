using Microsoft.Azure.Devices;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var iotHubConnectionString = "<Enter your IoT hub connection string>";

            var serviceClient = ServiceClient.CreateFromConnectionString(iotHubConnectionString);
            //var feedbackTask = ReceiveFeedback(serviceClient);

            Console.WriteLine("Initializing application to send messages from device...");

            Console.WriteLine("Which device do you wish to send messages to ?");
            Console.Write("> ");
            var deviceId = Console.ReadLine();

            while (true)
            {
                await SendCloudToDeviceMessage(serviceClient, deviceId);
                //await CallDirectMethod(serviceClient, deviceId);
            }
        }

        private static async Task SendCloudToDeviceMessage(ServiceClient serviceClient, string deviceId)
        {
            Console.WriteLine("What message payload do you want to send? ");
            Console.Write("> ");

            var payload = Console.ReadLine();

            var commandMessage = new Message(Encoding.ASCII.GetBytes(payload))
            {
                MessageId = Guid.NewGuid().ToString(),
                Ack = DeliveryAcknowledgement.Full,
                ExpiryTimeUtc = DateTime.UtcNow.AddSeconds(10)
            };

            await serviceClient.SendAsync(deviceId, commandMessage);
        }

        private static async Task ReceiveFeedback(ServiceClient serviceClient)
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();
            while (true)
            {
                var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                if (feedbackBatch == null)
                    continue;

                foreach (var record in feedbackBatch.Records)
                {
                    var messageId = record.OriginalMessageId;
                    var messageCode = record.StatusCode;

                    Console.WriteLine($"Feedback for message {messageId}, status code: {messageCode}. ");

                }
                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
        }

        private static async Task CallDirectMethod(ServiceClient serviceClient, string deviceId)
        {
            var method = new CloudToDeviceMethod("showMessage");
            method.SetPayloadJson("'Hello from C#'");

            var response = await serviceClient.InvokeDeviceMethodAsync(deviceId, method);
            Console.WriteLine($"Response status: {response.Status}, payload: {response.GetPayloadAsJson()}");
        }
    }
}

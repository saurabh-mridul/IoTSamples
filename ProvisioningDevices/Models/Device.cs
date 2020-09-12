using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client;
using Newtonsoft.Json;
using ProvisioningDevices.Interfaces;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProvisioningDevices.Models
{
    public class Device : IDevice
    {
        private DeviceClient _deviceClient;
        private readonly int _colorId;
        public Device()
        {
            _colorId = new Random().Next(1, 16);
        }

        public Device(string connectionString)
        {
            _deviceClient = DeviceClient.CreateFromConnectionString(connectionString);
        }

        public string Key { get; set; }

        public string DeviceId { get; set; }

        public string AssignedHub { get; set; }

        public ProvisioningRegistrationStatusType Status { get; set; }

        public void InitializeDevice()
        {
            if (Status != ProvisioningRegistrationStatusType.Assigned)
            {
                Console.WriteLine("Device is not assigned");
                return;
            }
            var auth = new DeviceAuthenticationWithRegistrySymmetricKey(DeviceId, Key);
            _deviceClient = DeviceClient.Create(AssignedHub, auth);
        }

        public async Task SendAsync<T>(T message)
         where T : class, new()
        {
            if (_deviceClient == null)
            {
                InitializeDevice();
            }
            var _message = JsonConvert.SerializeObject(message);
            var encodedMessage = new Message(Encoding.ASCII.GetBytes(_message));
            await _deviceClient.SendEventAsync(encodedMessage);
            Console.WriteLine("Message sent from device: " + DeviceId);
        }

        public async Task SendAsync<T>(T message, int messageInterval, int messageCount)
          where T : class, new()
        {
            if (_deviceClient == null)
            {
                InitializeDevice();
            }
            if (messageCount < 0)
            {
                while (true)
                {
                    var _message = JsonConvert.SerializeObject(message);
                    var encodedMessage = new Message(Encoding.ASCII.GetBytes(_message));
                    await _deviceClient.SendEventAsync(encodedMessage);
                    Console.ForegroundColor = GenerateConsoleColor();
                    Console.WriteLine("Message sent from device: " + DeviceId);
                    Thread.Sleep(messageInterval);
                }
            }
            else
            {
                while (messageCount > 0)
                {
                    var _message = JsonConvert.SerializeObject(message);
                    var encodedMessage = new Message(Encoding.ASCII.GetBytes(_message));
                    await _deviceClient.SendEventAsync(encodedMessage);
                    Console.WriteLine("Message sent from device: " + DeviceId);
                    messageCount--;
                    Thread.Sleep(messageInterval);
                }
            }
        }

        public Telemetry GenerateMessage()
        {
            return new Telemetry { Id = new Guid(), Message = "Hello World" };
        }

        private ConsoleColor GenerateConsoleColor()
        {
            return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), _colorId.ToString());
        }
    }
}

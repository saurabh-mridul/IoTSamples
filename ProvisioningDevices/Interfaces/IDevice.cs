using Microsoft.Azure.Devices.Provisioning.Client;
using ProvisioningDevices.Models;
using System.Threading.Tasks;

namespace ProvisioningDevices.Interfaces
{
    public interface IDevice
    {
        string Key { get; set; }

        string DeviceId { get; set; }

        string AssignedHub { get; set; }

        ProvisioningRegistrationStatusType Status { get; set; }

        Task SendAsync<T>(T message) where T : class, new();

        Task SendAsync<T>(T message, int messageInterval, int messageCount)
          where T : class, new();

        Telemetry GenerateMessage();
    }
}

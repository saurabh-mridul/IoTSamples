using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using ProvisioningDevices.Interfaces;
using ProvisioningDevices.Models;
using System;
using System.Threading.Tasks;

namespace ProvisioningDevices
{
    public class DeviceProvisioning : IDeviceProvisioning
    {
        private readonly DpsContext _dpsContext;
        public DeviceProvisioning(DpsContext dpsContext)
        {
            _dpsContext = dpsContext;
        }

        public async Task<IDevice> RegisterDevicesAsync(string deviceId)
        {
            var devicePrimaryKey = Utilities.ComputeDerivedSymmetricKey(Convert.FromBase64String(_dpsContext.PrimaryKey), deviceId);
            var deviceSecondaryKey = Utilities.ComputeDerivedSymmetricKey(Convert.FromBase64String(_dpsContext.SecondaryKey), deviceId);

            using (var security = new SecurityProviderSymmetricKey(deviceId, devicePrimaryKey, deviceSecondaryKey))
            {
                using (var transport = new ProvisioningTransportHandlerMqtt())
                {
                    Console.WriteLine($"Resgistering {deviceId}....");
                    var provisioningDeviceClient = ProvisioningDeviceClient.Create(_dpsContext.GlobalDeviceEndpoint, _dpsContext.ScopeId, security, transport);
                    var result = await provisioningDeviceClient.RegisterAsync();
                    var device = new Device
                    {
                        Key = security.GetPrimaryKey(),
                        DeviceId = result.DeviceId,
                        AssignedHub = result.AssignedHub,
                        Status = result.Status
                    };
                    return device;
                }
            }
        }
    }
}

using Microsoft.Azure.Devices.Provisioning.Client;
using ProvisioningDevices.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProvisioningDevices.Interfaces
{
    public interface IDeviceProvisioning
    {
        Task<IDevice> RegisterDevicesAsync(string deviceId);
    }
}

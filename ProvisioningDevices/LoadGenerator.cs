using ProvisioningDevices.Interfaces;
using ProvisioningDevices.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProvisioningDevices
{
    public class LoadGenerator
    {
        private static IDeviceProvisioning _deviceProvisioning;

        public static void RegisterDevicesAndStartSimulation(IDeviceProvisioning deviceProvisioning, int totalDevices)
        {
            _deviceProvisioning = deviceProvisioning;
            Task.Factory.StartNew(async () =>
            {
                for (int id = 1; id <= totalDevices; id++)
                {
                    var deviceName = "xdevice" + id;
                    var device = await _deviceProvisioning.RegisterDevicesAsync(deviceName);

                    // kick off simulation of a device immediately after registration.
                    StartAsync(device);
                }
            });
        }

        private static void StartAsync(IDevice device)
        {
            Task.Factory.StartNew(async () =>
            {
                var message = device.GenerateMessage();
                await device.SendAsync(message, 3, Timeout.Infinite);
            }, TaskCreationOptions.LongRunning);
        }


        //private async Task<List<IDevice>> RegisterDevicesAsync(int totalDevices)
        //{
        //    _registeredDevices = new List<IDevice>();

        //    for (int id = 1; id <= totalDevices; id++)
        //    {
        //        var deviceName = "xdevice" + id;
        //        var device = await _deviceProvisioning.RegisterDevicesAsync(deviceName);
        //        _registeredDevices.Add(device);

        //        // kick off simulation of a device immediately after registration.
        //        StartAsync(device);
        //    }
        //    return _registeredDevices;
        //}

        //public void Start()
        //{
        //    Parallel.ForEach(_registeredDevices, async (device) =>
        //    {
        //        var message = device.GenerateMessage();
        //        await device.SendAsync(message, 3, Timeout.Infinite);
        //    });
        //}

    }
}

using System;

namespace ProvisioningDevices
{
    class Program
    {
        private static DpsContext _dpsContext;

        static void Main(string[] args)
        {
            try
            {
                _dpsContext = new DpsContext
                {
                    ScopeId = Environment.GetEnvironmentVariable("scopeId"),
                    GlobalDeviceEndpoint = Environment.GetEnvironmentVariable("globalDeviceEndpoint"),
                    PrimaryKey = Environment.GetEnvironmentVariable("primaryKey"),
                    SecondaryKey = Environment.GetEnvironmentVariable("secondarykey")
                };

                var devicesToSpin = Environment.GetEnvironmentVariable("devicesToSpin");
                Console.WriteLine("Initializing dps...");
                var registrar = new DeviceProvisioning(_dpsContext);

                Console.WriteLine("Initiating load generator to simulate devices....");
                LoadGenerator.RegisterDevicesAndStartSimulation(registrar, Convert.ToInt32(devicesToSpin));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}

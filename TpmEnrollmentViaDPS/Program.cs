using Microsoft.Azure.Devices.Provisioning.Service;
using System;
using System.Threading.Tasks;

namespace TpmEnrollmentViaDPS
{
    class Program
    {
        private static readonly string dpsConnectionString = "<Enter your DPS connection string>";
        private static readonly string registrationId = "sample-registrationid-csharp";

        private static readonly string tpmEndorsementKey =
          "AToAAQALAAMAsgAgg3GXZ0SEs/gakMyNRqXXJP1S124GUgtk8qHaGzMUaaoABgCAAEMAEAgAAAAAAAEAymiv5Eb5yVMnw+4GKbZizoW/Pxn+aTuSXnRJOIzkAs3QuZ8clw6xDwLtyHSvruoeO7nlEpBuBkFEUVD8gOyG4xNUlOFe2xCwe/IR2rlzzzgw4mVmGLfC3wlGObkNLel4QCIZTCScu15LWPEE4td2ujwVoWwtKpi1U2p2A0e2//bUsrvIGA+Bq+qhfp3VEOzl+X4l94VVQQxJX7xXGNzCf8N2Ty1YKnozN+a0NJRrvMD1urqiXUzEkCd701qC15Makt0GRqfgj8cDZIf6y/kIk3Oux1rEJ5jBn47OhZpnXibR6hXVYL3MRpPUKzV1rCd8HD9+Lyp+zvziKQJTlxbLqQ==";

        // Optional parameters
        private const string optionalDeviceId = "myCSharpDevice";
        private const ProvisioningStatus optionalProvisioningStatus = ProvisioningStatus.Enabled;


        static async Task Main(string[] args)
        {
            await CreateOrUpdateIndividualEnrollmentAsync();
            Console.ReadLine();
        }

        private static async Task CreateOrUpdateIndividualEnrollmentAsync()
        {
            try
            {
                using (var provisioningServiceClient =
                 ProvisioningServiceClient.CreateFromConnectionString(dpsConnectionString))
                {
                    Console.WriteLine("Connected to sample provisioning service successfully.");
                    // Create a new individualEnrollment config
                    Console.WriteLine();
                    Attestation attestation = new TpmAttestation(tpmEndorsementKey);
                    IndividualEnrollment individualEnrollment = new IndividualEnrollment(registrationId, attestation)
                    {
                        DeviceId = optionalDeviceId,
                        ProvisioningStatus = optionalProvisioningStatus
                    };

                    // Create the individualEnrollment
                    Console.WriteLine("Adding new individualEnrollment...");
                    IndividualEnrollment individualEnrollmentResult =
                        await provisioningServiceClient.CreateOrUpdateIndividualEnrollmentAsync(individualEnrollment).ConfigureAwait(false);
                    Console.WriteLine("IndividualEnrollment created :");
                    Console.WriteLine(individualEnrollmentResult);

                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
        }
    }
}

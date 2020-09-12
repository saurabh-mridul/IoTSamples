using Microsoft.Azure.Devices.Provisioning.Service;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace X509EnrollmentGroupViaDPS
{
    class Program
    {
        private static readonly string dpsConnectionString = "<Enter your DPS connection string>";

        private static readonly string enrollmentGroupId = "x509GroupEnrollment";
        private static readonly string x509RootCertPath = Directory.GetCurrentDirectory() + "\\Certificates\\certificate.cer";

        static async Task Main(string[] args)
        {
            await CreateOrUpdateEnrollmentGroupAsync();
            Console.ReadLine();
        }

        private static async Task CreateOrUpdateEnrollmentGroupAsync()
        {
            using (var provisioningServiceClient = ProvisioningServiceClient.CreateFromConnectionString(dpsConnectionString))
            {
                Console.WriteLine("Connected to sample provisioning service successfully.");

                Console.WriteLine();
                // Create a new enrollmentGroup config
                Console.WriteLine("Creating a new enrollmentGroup...");
                var certificate = new X509Certificate2(x509RootCertPath);
                Attestation attestation = X509Attestation.CreateFromRootCertificates(certificate);
                EnrollmentGroup enrollmentGroup = new EnrollmentGroup(enrollmentGroupId, attestation)
                {
                    ProvisioningStatus = ProvisioningStatus.Enabled,
                };
                Console.WriteLine(enrollmentGroup);

                // Create the enrollmentGroup
                Console.WriteLine("Adding new enrollmentGroup...");
                EnrollmentGroup enrollmentGroupResult = await provisioningServiceClient.CreateOrUpdateEnrollmentGroupAsync(enrollmentGroup).ConfigureAwait(false);
                Console.WriteLine("EnrollmentGroup created :");
                Console.WriteLine(enrollmentGroupResult);
            }
        }
    }
}

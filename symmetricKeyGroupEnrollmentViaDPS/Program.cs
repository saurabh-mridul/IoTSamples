using Microsoft.Azure.Devices.Provisioning.Service;
using System;
using System.Threading.Tasks;

namespace SymmetricKeyGroupEnrollmentViaDPS
{

    class Program
    {
        private static readonly string dpsConnectionString = "<Enter your DPS connection string>";
        private static readonly string enrollmentGroupId = "symmetricKeyGroupEnrollment";

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
                Console.WriteLine("Creating a new enrollmentGroup...");
                Attestation attestation = new SymmetricKeyAttestation(string.Empty, string.Empty);
                EnrollmentGroup enrollmentGroup =
                        new EnrollmentGroup(enrollmentGroupId, attestation)
                        {
                            ProvisioningStatus = ProvisioningStatus.Enabled

                        };
                Console.WriteLine(enrollmentGroup);

                // Create the enrollmentGroup
                Console.WriteLine("Adding new enrollmentGroup...");
                EnrollmentGroup enrollmentGroupResult =
                    await provisioningServiceClient.CreateOrUpdateEnrollmentGroupAsync(enrollmentGroup).ConfigureAwait(false);
                Console.WriteLine("EnrollmentGroup created :");
                Console.WriteLine(enrollmentGroupResult);
            }
        }
    }

}

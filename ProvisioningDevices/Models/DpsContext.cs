using System;
using System.Collections.Generic;
using System.Text;

namespace ProvisioningDevices
{
    public class DpsContext
    {
        public string ScopeId { get; set; }
        public string GlobalDeviceEndpoint { get; set; }
        public string PrimaryKey { get; set; }
        public string SecondaryKey { get; set; }
    }
}

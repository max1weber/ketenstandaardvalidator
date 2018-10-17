using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nl.ketenstandaard.api.environment
{
    internal class ServiceEndpoint
    {
        public const string DevelopEndpoint = "https://api.ketenstandaard.nl/api/SalesValidation/Validate";
        public const string TestEndpoint = "https://api.ketenstandaard.nl/api/SalesValidation/Validate";
        public const string AcceptanceEndpoint = "https://api.ketenstandaard.nl/api/SalesValidation/Validate";

        public const string ProductionEndpoint = "https://api.ketenstandaard.nl/api/SalesValidation/Validate";

    }



}

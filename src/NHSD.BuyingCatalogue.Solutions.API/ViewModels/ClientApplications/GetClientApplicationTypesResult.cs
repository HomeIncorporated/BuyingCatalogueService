using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications
{
    public sealed class GetClientApplicationTypesResult
    {
        [JsonProperty("client-application-types")]
        public IEnumerable<string> ClientApplicationTypes { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetClientApplicationTypesResult"/> class.
        /// </summary>
        public GetClientApplicationTypesResult(IClientApplication clientApplication)
        {
            ClientApplicationTypes = clientApplication?.ClientApplicationTypes ?? new HashSet<string>();
        }
    }
}

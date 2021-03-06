﻿using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class GetSolutionSupplierResult
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        public GetSolutionSupplierResult(ISolutionSupplier solutionSupplier)
        {
            Description = solutionSupplier?.Summary;
            Link = solutionSupplier?.Url;
        }
    }
}

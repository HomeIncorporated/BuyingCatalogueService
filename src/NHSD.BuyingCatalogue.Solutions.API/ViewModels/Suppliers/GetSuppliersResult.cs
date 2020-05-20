﻿namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class GetSuppliersResult
    {
        public string SupplierId { get; set; }
        public string Name { get; set; }
        public AddressModel Address { get; set; }
        public PrimaryContactModel PrimaryContact { get; set; }
    }
}

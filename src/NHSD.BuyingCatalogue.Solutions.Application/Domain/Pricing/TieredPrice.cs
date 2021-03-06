﻿namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public sealed class TieredPrice
    {
        public int BandStart { get; set; }

        public int? BandEnd { get; set; }

        public decimal Price { get; set; }

        public TieredPrice(int bandStart, int? bandEnd, decimal price)
        {
            BandStart = bandStart;
            BandEnd = bandEnd;
            Price = price;
        }
    }
}

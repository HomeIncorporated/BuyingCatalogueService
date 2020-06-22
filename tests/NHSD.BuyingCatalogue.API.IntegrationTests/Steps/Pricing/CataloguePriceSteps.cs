﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Pricing
{
    [Binding]
    internal sealed class CataloguePriceSteps
    {
        private readonly Response _response;
        private readonly ScenarioContext _context;

        private const string pricingUrl = "http://localhost:5200/api/v1/solutions/{0}/pricing";
        private readonly string priceToken = "prices";

        public CataloguePriceSteps(Response response, ScenarioContext context)
        {
            _response = response;
            _context = context;
        }

        [Given(@"CataloguePrice exists")]
        public async Task GivenCataloguePriceExists(Table table)
        {
            IDictionary<string, int> cataloguePriceDictionary = new Dictionary<string, int>();

            foreach (var cataloguePrice in table.CreateSet<CataloguePriceTable>())
            {
                var price = CataloguePriceEntityBuilder.Create()
                    .WithCatalogueItemId(cataloguePrice.CatalogueItemId)
                    .WithPriceTypeId((int)cataloguePrice.CataloguePriceType)
                    .WithCurrencyCode(cataloguePrice.CurrencyCode)
                    .WithPrice(cataloguePrice.Price)
                    .WithPricingUnitId(cataloguePrice.PricingUnitId)
                    .WithTimeUnit((int)cataloguePrice.TimeUnit)
                    .Build();

                var cataloguePriceId = await price.InsertAsync<int>();
                
                if(!cataloguePriceDictionary.ContainsKey(price.CurrencyCode))
                    cataloguePriceDictionary.Add(price.CurrencyCode, cataloguePriceId);
            }

            _context[ScenarioContextKeys.CatalogueTierMapDictionary] = cataloguePriceDictionary;
        }

        [When(@"a GET request is made to retrieve the pricing with Solution ID (.*)")]
        public async Task WhenAGETRequestIsMadeToRetrieveThePricingWithSolutionID(string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, pricingUrl, solutionId));
        }

        [Then(@"Prices are returned")]
        public async Task ThenPricesAreReturned(Table table)
        {
            var expected = table.CreateSet<PriceResultTable>().ToList();

            var content = (await _response.ReadBody()).SelectToken(priceToken).Select(x => new PriceResultTable
            {
                Type = x.Value<string>("type"),
                CurrencyCode = x.Value<string>("currencyCode"),
                Price = x.Value<decimal?>("price")
            });

            content.Should().BeEquivalentTo(expected);
        }

        [Then(@"has Pricing Item Unit")]
        public async Task ThenHasPricingItemUnit(Table table)
        {
            var expected = table.CreateSet<ItemUnitTable>().ToList();
            var pricesToken = (await _response.ReadBody()).SelectToken(priceToken);

            const string itemUnitToken = "itemUnit";
            var content = pricesToken.Select(x => new ItemUnitTable
            {
                Name = x.SelectToken(itemUnitToken).Value<string>("name"),
                Description = x.SelectToken(itemUnitToken).Value<string>("description"),
                TierName = x.SelectToken(itemUnitToken).Value<string>("tierName")
            });

            content.Should().BeEquivalentTo(expected);
        }

        [Then(@"has Pricing Time Unit")]
        public async Task ThenHasPricingTimeUnit(Table table)
        {
            var expected = table.CreateSet<TimeUnitTable>().ToList();
            var pricesToken = (await _response.ReadBody()).SelectToken(priceToken);

            const string timeUnitToken = "timeUnit";
            var content = pricesToken.Select(x => new TimeUnitTable
            {
                Name = x.SelectToken(timeUnitToken).Value<string>("name"),
                Description = x.SelectToken(timeUnitToken).Value<string>("description")
            });

            content.Should().BeEquivalentTo(expected);
        }

        [Then(@"the Prices Tiers are returned")]
        public async Task ThenThePricesTiersAreReturned(Table table)
        {
            var expected = table.CreateSet<TierTable>().ToList();
            var listExpected = expected.GroupBy(x => x.Section);

            var a = listExpected.Select(x => x.Select(y => new
            {
                Start = y.Start,
                End = y.End,
                Price = y.Price
            }));

            var pricesToken = (await _response.ReadBody()).SelectToken(priceToken);

            const string tierToken = "tiers";


            var content = pricesToken.Select(x => new
            {
                Tier = x.SelectToken(tierToken).Select(z => new
                {
                    Start = z.Value<int>("start"),
                    End = z.Value<int?>("end"),
                    Price = z.Value<decimal>("price")
                })
            });

            var A = content.Select(x => x.Tier);

            content.Select(x => x.Tier).Should().BeEquivalentTo(a, x => x.WithoutStrictOrdering());

            //content.SelectMany(x => x.Tier).Should().BeEquivalentTo(expected);
        }

        private sealed class CataloguePriceTable
        {
            public string CatalogueItemId { get; set; }
            public CataloguePriceType CataloguePriceType { get; set; }
            public string CurrencyCode { get; set; }
            public decimal? Price { get; set; }
            public Guid PricingUnitId { get; set; }
            public TimeUnit TimeUnit { get; set; }
        }

        private sealed class PriceResultTable
        {
            public string Type { get; set; }
            public string CurrencyCode { get; set; }
            public decimal? Price { get; set; }
        }

        private sealed class ItemUnitTable
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string TierName { get; set; }
        }

        private sealed class TimeUnitTable
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        private sealed class TierTable
        {
            public int Start { get; set; }
            public int? End { get; set; }
            public decimal Price { get; set; }
            public int Section { get; set; }
        }

        private enum CataloguePriceType
        {
            Flat = 1,
            Tiered = 2
        }

        private enum TimeUnit
        {
            Month = 1,
            Year = 2
        }
    }
}

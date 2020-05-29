﻿using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Persistence
{
    [TestFixture]
    internal sealed class SupplierReaderTests
    {
        [Test]
        public async Task ByIdAsync_ReturnsExpectedValues()
        {
            const string supplier = "10000";

            var mockResult = new Mock<ISupplierResult>();
            mockResult.Setup(r => r.Id).Returns(supplier);
            mockResult.Setup(r => r.Name).Returns("Kool Korp");
            mockResult.Setup(r => r.AddressLine1).Returns("Address line 1");
            mockResult.Setup(r => r.PrimaryContactFirstName).Returns("Bob");
            mockResult.Setup(r => r.HasAddress).Returns(true);
            mockResult.Setup(r => r.HasContact).Returns(true);

            var expectedSupplier = mockResult.Object;

            var mockRepo = new Mock<ISupplierRepository>();
            mockRepo.Setup(r => r.GetSupplierById(supplier, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSupplier);

            var reader = new SupplierReader(mockRepo.Object);

            var actualSupplier = await reader.ByIdAsync(supplier, new CancellationToken());

            actualSupplier.Id.Should().BeEquivalentTo(expectedSupplier.Id);
            actualSupplier.Name.Should().BeEquivalentTo(expectedSupplier.Name);
            actualSupplier.Address.Line1.Should().BeEquivalentTo(expectedSupplier.AddressLine1);
            actualSupplier.PrimaryContact.FirstName.Should().BeEquivalentTo(expectedSupplier.PrimaryContactFirstName);
        }

        [Test]
        public async Task ByNameAsync_ReturnsExpectedValues()
        {
            const string supplierName = "Supplier";

            static ISupplierResult MockSupplierNameResult(string id, string name)
            {
                var mockResult = new Mock<ISupplierResult>();
                mockResult.Setup(r => r.Id).Returns(id);
                mockResult.Setup(r => r.Name).Returns(name);

                return mockResult.Object;
            }

            var expectedSuppliers = new[]
            {
                MockSupplierNameResult("1", "Supplier 1"),
                MockSupplierNameResult("2", "Supplier 2")
            };

            var mockRepo = new Mock<ISupplierRepository>();
            mockRepo.Setup(
                r => r.GetSuppliersByName(supplierName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSuppliers);

            var reader = new SupplierReader(mockRepo.Object);

            var actualSuppliers = await reader.ByNameAsync(supplierName, new CancellationToken());

            actualSuppliers.Should().BeEquivalentTo(expectedSuppliers, config => config.ExcludingMissingMembers());
        }
    }
}

using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NUnit.Framework;
using FluentAssertions;
using NHSD.BuyingCatalogue.Contracts;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Domain.Tests.Solutions
{
    [TestFixture]
	public class SolutionTests
	{
        [Test]
        public void GivenSolution_CheckSupplierStatus_ShouldBeEqualToDraft()
        {
            //Arrange
            var expected = SupplierStatus.Draft;

            //Act
            var solution = new Solution();

            //Assert
            solution.SupplierStatus.Should().Be(expected);
        }
    }
}

using FluentAssertions;
using Refactor.Application.Services;

namespace Refactor.Application.Test.Services;

public class TaxServiceTests
{
    [TestCase(100, 19, 19)]
    [TestCase(50, 7.5, 3.75)]
    public void Should_Calculate_Tax(decimal net, decimal rate, decimal expectedTaxAmount)
    {
        // Arrange
        var sut = new TaxService();

        // Act
        var (taxAmount, _) = sut.CalculateTax(net, rate);

        // Assert
        taxAmount.Should().Be(expectedTaxAmount);
    }
}

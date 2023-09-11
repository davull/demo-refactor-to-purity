using FluentAssertions;
using Refactor.Application.Services;

namespace Refactor.Application.Test.Services;

public class TaxServiceTests
{
    [TestCase(100, 19, 19)]
    [TestCase(50, 7.5, 3.75)]
    public void Should_Calculate_Tax(decimal net, decimal rate, decimal expectedTaxAmount)
    {
        var (taxAmount, _) = TaxService.CalculateTax(net, rate);

        taxAmount.Should().Be(expectedTaxAmount);
    }
}

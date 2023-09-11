namespace Refactor.Application.Services;

public static class TaxService
{
    public static decimal DefaultTaxRate = 19m;

    public static (decimal taxAmount, decimal grossPrice) CalculateTax(decimal netPrice, decimal taxRate)
    {
        var taxAmount = netPrice * taxRate / 100m;
        var grossPrice = netPrice + taxAmount;

        return (taxAmount, grossPrice);
    }
}

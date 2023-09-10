namespace Refactor.Application.Services;

public class TaxService : ITaxService
{
    public (decimal taxAmount, decimal grossPrice) CalculateTax(decimal netPrice, decimal taxRate)
    {
        var taxAmount = netPrice * taxRate / 100m;
        var grossPrice = netPrice + taxAmount;

        return (taxAmount, grossPrice);
    }
}

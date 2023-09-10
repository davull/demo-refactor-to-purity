namespace Refactor.Application.Services;

public interface ITaxService
{
    (decimal taxAmount, decimal grossPrice) CalculateTax(decimal netPrice, decimal taxRate);
}

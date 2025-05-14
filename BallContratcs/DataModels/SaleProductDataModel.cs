using BallContratcs.Exceptions;
using BallContratcs.Extensions;
using BallContratcs.Infrastructure;

namespace BallContratcs.DataModels;

public class SaleProductDataModel(string saleId, string productId, int count) : IValidation
{
	public string SaleId { get; private set; } = saleId;

	public string ProductId { get; private set; } = productId;

	public int Count { get; private set; } = count;

	public void Validate()
	{
		if (SaleId.IsEmpty())
			throw new ValidationException("Field SaleId is empty");

		if (!SaleId.IsGuid())
			throw new ValidationException("The value in the field SaleId is not a unique identifier");

		if (ProductId.IsEmpty())
			throw new ValidationException("Field ProductId is empty");

		if (!ProductId.IsGuid())
			throw new ValidationException("The value in the field ProductId is not a unique identifier");

		if (Count <= 0)
			throw new ValidationException("Field Count is less than or equal to 0");
	}
}
using BallContratcs.Enums;
using BallContratcs.Exceptions;
using BallContratcs.Extensions;
using BallContratcs.Infrastructure;

namespace BallContratcs.DataModels;

public class SaleDataModel(string id, string workerId, string? buyerId, double sum, DiscountType discountType, double discount, bool isCancel, List<SaleProductDataModel> products) : IValidation
{
	public string Id { get; private set; } = id;

	public string WorkerId { get; private set; } = workerId;

	public string? BuyerId { get; private set; } = buyerId;

	public DateTime SaleDate { get; private set; } = DateTime.UtcNow;

	public double Sum { get; private set; } = sum;

	public DiscountType DiscountType { get; private set; } = discountType;

	public double Discount { get; private set; } = discount;

	public bool IsCancel { get; private set; } = isCancel;

	public List<SaleProductDataModel> Products { get; private set; } = products;

	public void Validate()
	{
		if (Id.IsEmpty())
			throw new ValidationException("Field Id is empty");

		if (!Id.IsGuid())
			throw new ValidationException("The value in the field Id is not a unique identifier");

		if (WorkerId.IsEmpty())
			throw new ValidationException("Field WorkerId is empty");

		if (!WorkerId.IsGuid())
			throw new ValidationException("The value in the field WorkerId is not a unique identifier");

		if (!BuyerId?.IsGuid() ?? !BuyerId?.IsEmpty() ?? false)
			throw new ValidationException("The value in the field BuyerId is not a unique identifier");

		if (Sum <= 0)
			throw new ValidationException("Field Sum is less than or equal to 0");

		if ((Products?.Count ?? 0) == 0)
			throw new ValidationException("The sale must include products");
	}
}
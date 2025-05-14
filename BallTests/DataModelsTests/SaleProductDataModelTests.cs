using BallContratcs.DataModels;
using BallContratcs.Exceptions;

namespace BallTests.DataModelsTests;

[TestFixture]
internal class SaleProductDataModelTests
{
	[Test]
	public void SaleIdIsNullOrEmptyTest()
	{
		var saleProduct = CreateDataModel(null, Guid.NewGuid().ToString(), 10);
		Assert.That(() => saleProduct.Validate(), Throws.TypeOf<ValidationException>());
		saleProduct = CreateDataModel(string.Empty, Guid.NewGuid().ToString(), 10);
		Assert.That(() => saleProduct.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void SaleIdIsNotGuidTest()
	{
		var saleProduct = CreateDataModel("saleId", Guid.NewGuid().ToString(), 10);
		Assert.That(() => saleProduct.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void ProductIdIsNullOrEmptyTest()
	{
		var saleProduct = CreateDataModel(Guid.NewGuid().ToString(), null, 10);
		Assert.That(() => saleProduct.Validate(), Throws.TypeOf<ValidationException>());
		saleProduct = CreateDataModel(string.Empty, Guid.NewGuid().ToString(), 10);
		Assert.That(() => saleProduct.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void ProductIdIsNotGuidTest()
	{
		var saleProduct = CreateDataModel(Guid.NewGuid().ToString(), "productId", 10);
		Assert.That(() => saleProduct.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void CountIsLessOrZeroTest()
	{
		var saleProduct = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0);
		Assert.That(() => saleProduct.Validate(), Throws.TypeOf<ValidationException>());
		saleProduct = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), -10);
		Assert.That(() => saleProduct.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void AllFieldsIsCorrectTest()
	{
		var saleId = Guid.NewGuid().ToString();
		var productId = Guid.NewGuid().ToString();
		var count = 10;
		var saleProduct = CreateDataModel(saleId, productId, count);
		Assert.That(() => saleProduct.Validate(), Throws.Nothing);
		Assert.Multiple(() =>
		{
			Assert.That(saleProduct.SaleId, Is.EqualTo(saleId));
			Assert.That(saleProduct.ProductId, Is.EqualTo(productId));
			Assert.That(saleProduct.Count, Is.EqualTo(count));
		});
	}

	private static SaleProductDataModel CreateDataModel(string? saleId, string? productId, int count) =>
		new(saleId, productId, count);
}
﻿using BallContratcs.DataModels;
using BallContratcs.Enums;
using BallContratcs.Exceptions;

namespace BallTests.DataModelsTests;

[TestFixture]
internal class ProductDataModelTests
{
	[Test]
	public void IdIsNullOrEmptyTest()
	{
		var product = CreateDataModel(null, "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false);
		Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
		product = CreateDataModel(string.Empty, "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false);
		Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void IdIsNotGuidTest()
	{
		var product = CreateDataModel("id", "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false);
		Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void ProductNameIsEmptyTest()
	{
		var product = CreateDataModel(Guid.NewGuid().ToString(), null, ProductType.Accessory, Guid.NewGuid().ToString(), 10, false);
		Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
		product = CreateDataModel(Guid.NewGuid().ToString(), string.Empty, ProductType.Accessory, Guid.NewGuid().ToString(), 10, false);
		Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void ProductTypeIsNoneTest()
	{
		var product = CreateDataModel(Guid.NewGuid().ToString(), null, ProductType.None, Guid.NewGuid().ToString(), 10, false);
		Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void ManufacturerIdIsNullOrEmptyTest()
	{
		var product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.Accessory, null, 10, false);
		Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
		product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.Accessory, string.Empty, 10, false);
		Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void ManufacturerIdIsNotGuidTest()
	{
		var product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.Accessory, "manufacturerId", 10, false);
		Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void PriceIsLessOrZeroTest()
	{
		var product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.Accessory, Guid.NewGuid().ToString(), 0, false);
		Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
		product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.Accessory, Guid.NewGuid().ToString(), -10, false);
		Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void AllFieldsIsCorrectTest()
	{
		var productId = Guid.NewGuid().ToString();
		var productName = "name";
		var productType = ProductType.Accessory;
		var productManufacturerId = Guid.NewGuid().ToString();
		var productPrice = 10;
		var productIsDelete = false;
		var product = CreateDataModel(productId, productName, productType, productManufacturerId, productPrice, productIsDelete);
		Assert.That(() => product.Validate(), Throws.Nothing);
		Assert.Multiple(() =>
		{
			Assert.That(product.Id, Is.EqualTo(productId));
			Assert.That(product.ProductName, Is.EqualTo(productName));
			Assert.That(product.ProductType, Is.EqualTo(productType));
			Assert.That(product.ManufacturerId, Is.EqualTo(productManufacturerId));
			Assert.That(product.Price, Is.EqualTo(productPrice));
			Assert.That(product.IsDeleted, Is.EqualTo(productIsDelete));
		});
	}

	private static ProductDataModel CreateDataModel(string? id, string? productName, ProductType productType, string? manufacturerId, double price, bool isDeleted) =>
		new(id, productName, productType, manufacturerId, price, isDeleted);
}
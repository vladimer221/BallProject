using BallBusinessLogic.Implementations;
using BallContratcs.DataModels;
using BallContratcs.Enums;
using BallContratcs.Exceptions;
using BallContratcs.StoragesContracts;
using Microsoft.Extensions.Logging;
using Moq;

namespace BallTests.BusinessLogicsContractsTests;

[TestFixture]
internal class ProductBusinessLogicContractTests
{
	private ProductBusinessLogicContract _productBusinessLogicContract;
	private Mock<IProductStorageContract> _productStorageContract;

	[OneTimeSetUp]
	public void OneTimeSetUp()
	{
		_productStorageContract = new Mock<IProductStorageContract>();
		_productBusinessLogicContract = new ProductBusinessLogicContract(_productStorageContract.Object, new Mock<ILogger>().Object);
	}

	[SetUp]
	public void SetUp()
	{
		_productStorageContract.Reset();
	}

	[Test]
	public void GetAllProducts_ReturnListOfRecords_Test()
	{
		//Arrange
		var listOriginal = new List<ProductDataModel>()
		{
			new(Guid.NewGuid().ToString(), "name 1", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false),
			new(Guid.NewGuid().ToString(), "name 2", ProductType.Accessory, Guid.NewGuid().ToString(), 10, true),
			new(Guid.NewGuid().ToString(), "name 3", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false),
		};
		_productStorageContract.Setup(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>())).Returns(listOriginal);
		//Act
		var listOnlyActive = _productBusinessLogicContract.GetAllProducts(true);
		var list = _productBusinessLogicContract.GetAllProducts(false);
		//Assert
		Assert.Multiple(() =>
		{
			Assert.That(listOnlyActive, Is.Not.Null);
			Assert.That(list, Is.Not.Null);
			Assert.That(listOnlyActive, Is.EquivalentTo(listOriginal));
			Assert.That(list, Is.EquivalentTo(listOriginal));
		});
		_productStorageContract.Verify(x => x.GetList(true, null), Times.Once);
		_productStorageContract.Verify(x => x.GetList(false, null), Times.Once);
	}

	[Test]
	public void GetAllProducts_ReturnEmptyList_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>())).Returns([]);
		//Act
		var listOnlyActive = _productBusinessLogicContract.GetAllProducts(true);
		var list = _productBusinessLogicContract.GetAllProducts(false);
		//Assert
		Assert.Multiple(() =>
		{
			Assert.That(listOnlyActive, Is.Not.Null);
			Assert.That(list, Is.Not.Null);
			Assert.That(listOnlyActive, Has.Count.EqualTo(0));
			Assert.That(list, Has.Count.EqualTo(0));
		});
		_productStorageContract.Verify(x => x.GetList(It.IsAny<bool>(), null), Times.Exactly(2));
	}

	[Test]
	public void GetAllProducts_ReturnNull_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetAllProducts(It.IsAny<bool>()), Throws.TypeOf<NullListException>());
		_productStorageContract.Verify(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void GetAllProducts_StorageThrowError_ThrowException_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetAllProducts(It.IsAny<bool>()), Throws.TypeOf<StorageException>());
		_productStorageContract.Verify(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void GetAllProductsByManufacturer_ReturnListOfRecords_Test()
	{
		//Arrange
		var manufacturerId = Guid.NewGuid().ToString();
		var listOriginal = new List<ProductDataModel>()
		{
			new(Guid.NewGuid().ToString(), "name 1",ProductType.Accessory, Guid.NewGuid().ToString(), 10, false),
			new(Guid.NewGuid().ToString(), "name 2", ProductType.Accessory, Guid.NewGuid().ToString(), 10, true),
			new(Guid.NewGuid().ToString(), "name 3", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false),
		};
		_productStorageContract.Setup(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>())).Returns(listOriginal);
		//Act
		var listOnlyActive = _productBusinessLogicContract.GetAllProductsByManufacturer(manufacturerId, true);
		var list = _productBusinessLogicContract.GetAllProductsByManufacturer(manufacturerId, false);
		//Assert
		Assert.Multiple(() =>
		{
			Assert.That(listOnlyActive, Is.Not.Null);
			Assert.That(list, Is.Not.Null);
			Assert.That(listOnlyActive, Is.EquivalentTo(listOriginal));
			Assert.That(list, Is.EquivalentTo(listOriginal));
		});
		_productStorageContract.Verify(x => x.GetList(true, manufacturerId), Times.Once);
		_productStorageContract.Verify(x => x.GetList(false, manufacturerId), Times.Once);
	}

	[Test]
	public void GetAllProductsByManufacturer_ReturnEmptyList_Test()
	{
		//Arrange
		var manufacturerId = Guid.NewGuid().ToString();
		_productStorageContract.Setup(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>())).Returns([]);
		//Act
		var listOnlyActive = _productBusinessLogicContract.GetAllProductsByManufacturer(manufacturerId, true);
		var list = _productBusinessLogicContract.GetAllProductsByManufacturer(manufacturerId, false);
		//Assert
		Assert.Multiple(() =>
		{
			Assert.That(listOnlyActive, Is.Not.Null);
			Assert.That(list, Is.Not.Null);
			Assert.That(listOnlyActive, Has.Count.EqualTo(0));
			Assert.That(list, Has.Count.EqualTo(0));
		});
		_productStorageContract.Verify(x => x.GetList(It.IsAny<bool>(), manufacturerId), Times.Exactly(2));
	}

	[Test]
	public void GetAllProductsByManufacturer_ManufacturerIdIsNullOrEmpty_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetAllProductsByManufacturer(null, It.IsAny<bool>()), Throws.TypeOf<ArgumentNullException>());
		Assert.That(() => _productBusinessLogicContract.GetAllProductsByManufacturer(string.Empty, It.IsAny<bool>()), Throws.TypeOf<ArgumentNullException>());
		_productStorageContract.Verify(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>()), Times.Never);
	}

	[Test]
	public void GetAllProductsByManufacturer_ManufacturerIdIsNotGuid_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetAllProductsByManufacturer("manufacturerId", It.IsAny<bool>()), Throws.TypeOf<ValidationException>());
		_productStorageContract.Verify(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>()), Times.Never);
	}

	[Test]
	public void GetAllProductsByManufacturer_ReturnNull_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetAllProductsByManufacturer(Guid.NewGuid().ToString(), It.IsAny<bool>()), Throws.TypeOf<NullListException>());
		_productStorageContract.Verify(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void GetAllProductsByManufacturer_StorageThrowError_ThrowException_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetAllProductsByManufacturer(Guid.NewGuid().ToString(), It.IsAny<bool>()), Throws.TypeOf<StorageException>());
		_productStorageContract.Verify(x => x.GetList(It.IsAny<bool>(), It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void GetProductHistoryByProduct_ReturnListOfRecords_Test()
	{
		//Arrange
		var productId = Guid.NewGuid().ToString();
		var listOriginal = new List<ProductHistoryDataModel>()
		{
			new(Guid.NewGuid().ToString(), 10),
			new(Guid.NewGuid().ToString(), 15),
			new(Guid.NewGuid().ToString(), 10),
		};
		_productStorageContract.Setup(x => x.GetHistoryByProductId(It.IsAny<string>())).Returns(listOriginal);
		//Act
		var list = _productBusinessLogicContract.GetProductHistoryByProduct(productId);
		//Assert
		Assert.That(list, Is.Not.Null);
		Assert.That(list, Is.EquivalentTo(listOriginal));
		_productStorageContract.Verify(x => x.GetHistoryByProductId(productId), Times.Once);
	}

	[Test]
	public void GetProductHistoryByProduct_ReturnEmptyList_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.GetHistoryByProductId(It.IsAny<string>())).Returns([]);
		//Act
		var list = _productBusinessLogicContract.GetProductHistoryByProduct(Guid.NewGuid().ToString());
		//Assert
		Assert.That(list, Is.Not.Null);
		Assert.That(list, Has.Count.EqualTo(0));
		_productStorageContract.Verify(x => x.GetHistoryByProductId(It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void GetProductHistoryByProduct_ProductIdIsNullOrEmpty_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetProductHistoryByProduct(null), Throws.TypeOf<ArgumentNullException>());
		Assert.That(() => _productBusinessLogicContract.GetProductHistoryByProduct(string.Empty), Throws.TypeOf<ArgumentNullException>());
		_productStorageContract.Verify(x => x.GetHistoryByProductId(It.IsAny<string>()), Times.Never);
	}

	[Test]
	public void GetProductHistoryByProduct_ProductIdIsNotGuid_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetProductHistoryByProduct("productId"), Throws.TypeOf<ValidationException>());
		_productStorageContract.Verify(x => x.GetHistoryByProductId(It.IsAny<string>()), Times.Never);
	}

	[Test]
	public void GetProductHistoryByProduct_ReturnNull_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetProductHistoryByProduct(Guid.NewGuid().ToString()), Throws.TypeOf<NullListException>());
		_productStorageContract.Verify(x => x.GetHistoryByProductId(It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void GetProductHistoryByProduct_StorageThrowError_ThrowException_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.GetHistoryByProductId(It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetProductHistoryByProduct(Guid.NewGuid().ToString()), Throws.TypeOf<StorageException>());
		_productStorageContract.Verify(x => x.GetHistoryByProductId(It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void GetProductByData_GetById_ReturnRecord_Test()
	{
		//Arrange
		var id = Guid.NewGuid().ToString();
		var record = new ProductDataModel(id, "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false);
		_productStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
		//Act
		var element = _productBusinessLogicContract.GetProductByData(id);
		//Assert
		Assert.That(element, Is.Not.Null);
		Assert.That(element.Id, Is.EqualTo(id));
		_productStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void GetProductByData_GetByName_ReturnRecord_Test()
	{
		//Arrange
		var name = "name";
		var record = new ProductDataModel(Guid.NewGuid().ToString(), name, ProductType.Accessory, Guid.NewGuid().ToString(), 10, false);
		_productStorageContract.Setup(x => x.GetElementByName(name)).Returns(record);
		//Act
		var element = _productBusinessLogicContract.GetProductByData(name);
		//Assert
		Assert.That(element, Is.Not.Null);
		Assert.That(element.ProductName, Is.EqualTo(name));
		_productStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void GetProductByData_EmptyData_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetProductByData(null), Throws.TypeOf<ArgumentNullException>());
		Assert.That(() => _productBusinessLogicContract.GetProductByData(string.Empty), Throws.TypeOf<ArgumentNullException>());
		_productStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Never);
		_productStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Never);
	}

	[Test]
	public void GetProductByData_GetById_NotFoundRecord_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetProductByData(Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
		_productStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void GetProductByData_GetByName_NotFoundRecord_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetProductByData("name"), Throws.TypeOf<ElementNotFoundException>());
		_productStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void GetProductByData_StorageThrowError_ThrowException_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.GetElementById(It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
		_productStorageContract.Setup(x => x.GetElementByName(It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.GetProductByData(Guid.NewGuid().ToString()), Throws.TypeOf<StorageException>());
		Assert.That(() => _productBusinessLogicContract.GetProductByData("name"), Throws.TypeOf<StorageException>());
		_productStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
		_productStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void InsertProduct_CorrectRecord_Test()
	{
		//Arrange
		var flag = false;
		var record = new ProductDataModel(Guid.NewGuid().ToString(), "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false);
		_productStorageContract.Setup(x => x.AddElement(It.IsAny<ProductDataModel>()))
			.Callback((ProductDataModel x) =>
			{
				flag = x.Id == record.Id && x.ProductName == record.ProductName && x.ProductType == record.ProductType &&
				x.ManufacturerId == record.ManufacturerId && x.Price == record.Price && x.IsDeleted == record.IsDeleted;
			});
		//Act
		_productBusinessLogicContract.InsertProduct(record);
		//Assert
		_productStorageContract.Verify(x => x.AddElement(It.IsAny<ProductDataModel>()), Times.Once);
		Assert.That(flag);
	}

	[Test]
	public void InsertProduct_RecordWithExistsData_ThrowException_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.AddElement(It.IsAny<ProductDataModel>())).Throws(new ElementExistsException("Data", "Data"));
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.InsertProduct(new(Guid.NewGuid().ToString(), "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false)), Throws.TypeOf<ElementExistsException>());
		_productStorageContract.Verify(x => x.AddElement(It.IsAny<ProductDataModel>()), Times.Once);
	}

	[Test]
	public void InsertProduct_NullRecord_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.InsertProduct(null), Throws.TypeOf<ArgumentNullException>());
		_productStorageContract.Verify(x => x.AddElement(It.IsAny<ProductDataModel>()), Times.Never);
	}

	[Test]
	public void InsertProduct_InvalidRecord_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.InsertProduct(new ProductDataModel("id", "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false)), Throws.TypeOf<ValidationException>());
		_productStorageContract.Verify(x => x.AddElement(It.IsAny<ProductDataModel>()), Times.Never);
	}

	[Test]
	public void InsertProduct_StorageThrowError_ThrowException_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.AddElement(It.IsAny<ProductDataModel>())).Throws(new StorageException(new InvalidOperationException()));
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.InsertProduct(new(Guid.NewGuid().ToString(), "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false)), Throws.TypeOf<StorageException>());
		_productStorageContract.Verify(x => x.AddElement(It.IsAny<ProductDataModel>()), Times.Once);
	}

	[Test]
	public void UpdateProduct_CorrectRecord_Test()
	{
		//Arrange
		var flag = false;
		var record = new ProductDataModel(Guid.NewGuid().ToString(), "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false);
		_productStorageContract.Setup(x => x.UpdElement(It.IsAny<ProductDataModel>()))
			.Callback((ProductDataModel x) =>
			{
				flag = x.Id == record.Id && x.ProductName == record.ProductName && x.ProductType == record.ProductType &&
				x.ManufacturerId == record.ManufacturerId && x.Price == record.Price && x.IsDeleted == record.IsDeleted;
			});
		//Act
		_productBusinessLogicContract.UpdateProduct(record);
		//Assert
		_productStorageContract.Verify(x => x.UpdElement(It.IsAny<ProductDataModel>()), Times.Once);
		Assert.That(flag);
	}

	[Test]
	public void UpdateProduct_RecordWithIncorrectData_ThrowException_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.UpdElement(It.IsAny<ProductDataModel>())).Throws(new ElementNotFoundException(""));
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.UpdateProduct(new(Guid.NewGuid().ToString(), "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false)), Throws.TypeOf<ElementNotFoundException>());
		_productStorageContract.Verify(x => x.UpdElement(It.IsAny<ProductDataModel>()), Times.Once);
	}

	[Test]
	public void UpdateProduct_RecordWithExistsData_ThrowException_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.UpdElement(It.IsAny<ProductDataModel>())).Throws(new ElementExistsException("Data", "Data"));
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.UpdateProduct(new(Guid.NewGuid().ToString(), "anme", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false)), Throws.TypeOf<ElementExistsException>());
		_productStorageContract.Verify(x => x.UpdElement(It.IsAny<ProductDataModel>()), Times.Once);
	}

	[Test]
	public void UpdateProduct_NullRecord_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.UpdateProduct(null), Throws.TypeOf<ArgumentNullException>());
		_productStorageContract.Verify(x => x.UpdElement(It.IsAny<ProductDataModel>()), Times.Never);
	}

	[Test]
	public void UpdateProduct_InvalidRecord_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.UpdateProduct(new ProductDataModel("id", "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false)), Throws.TypeOf<ValidationException>());
		_productStorageContract.Verify(x => x.UpdElement(It.IsAny<ProductDataModel>()), Times.Never);
	}

	[Test]
	public void UpdateProduct_StorageThrowError_ThrowException_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.UpdElement(It.IsAny<ProductDataModel>())).Throws(new StorageException(new InvalidOperationException()));
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.UpdateProduct(new(Guid.NewGuid().ToString(), "name", ProductType.Accessory, Guid.NewGuid().ToString(), 10, false)), Throws.TypeOf<StorageException>());
		_productStorageContract.Verify(x => x.UpdElement(It.IsAny<ProductDataModel>()), Times.Once);
	}

	[Test]
	public void DeleteProduct_CorrectRecord_Test()
	{
		//Arrange
		var id = Guid.NewGuid().ToString();
		var flag = false;
		_productStorageContract.Setup(x => x.DelElement(It.Is((string x) => x == id))).Callback(() => { flag = true; });
		//Act
		_productBusinessLogicContract.DeleteProduct(id);
		//Assert
		_productStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
		Assert.That(flag);
	}

	[Test]
	public void DeleteProduct_RecordWithIncorrectId_ThrowException_Test()
	{
		//Arrange
		var id = Guid.NewGuid().ToString();
		_productStorageContract.Setup(x => x.DelElement(It.IsAny<string>())).Throws(new ElementNotFoundException(id));
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.DeleteProduct(Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
		_productStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
	}

	[Test]
	public void DeleteProduct_IdIsNullOrEmpty_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.DeleteProduct(null), Throws.TypeOf<ArgumentNullException>());
		Assert.That(() => _productBusinessLogicContract.DeleteProduct(string.Empty), Throws.TypeOf<ArgumentNullException>());
		_productStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
	}

	[Test]
	public void DeleteProduct_IdIsNotGuid_ThrowException_Test()
	{
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.DeleteProduct("id"), Throws.TypeOf<ValidationException>());
		_productStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
	}

	[Test]
	public void DeleteProduct_StorageThrowError_ThrowException_Test()
	{
		//Arrange
		_productStorageContract.Setup(x => x.DelElement(It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
		//Act&Assert
		Assert.That(() => _productBusinessLogicContract.DeleteProduct(Guid.NewGuid().ToString()), Throws.TypeOf<StorageException>());
		_productStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
	}
}
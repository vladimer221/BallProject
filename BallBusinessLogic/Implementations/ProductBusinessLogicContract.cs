using BallContratcs.BusinessLogicsContracts;
using BallContratcs.DataModels;
using BallContratcs.Exceptions;
using BallContratcs.Extensions;
using BallContratcs.StoragesContracts;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BallBusinessLogic.Implementations;

internal class ProductBusinessLogicContract(IProductStorageContract productStorageContract, ILogger logger) : IProductBusinessLogicContract
{
	private readonly ILogger _logger = logger;
	private readonly IProductStorageContract _productStorageContract = productStorageContract;

	public List<ProductDataModel> GetAllProducts(bool onlyActive)
	{
		_logger.LogInformation("GetAllProducts params: {onlyActive}", onlyActive);
		return _productStorageContract.GetList(onlyActive) ?? throw new NullListException();
	}

	public List<ProductDataModel> GetAllProductsByManufacturer(string manufacturerId, bool onlyActive = true)
	{
		if (manufacturerId.IsEmpty())
		{
			throw new ArgumentNullException(nameof(manufacturerId));
		}
		if (!manufacturerId.IsGuid())
		{
			throw new ValidationException("The value in the field manufacturerId is not a unique identifier.");
		}
		_logger.LogInformation("GetAllProducts params: {manufacturerId}, {onlyActive}", manufacturerId, onlyActive);
		return _productStorageContract.GetList(onlyActive, manufacturerId) ?? throw new NullListException();
	}

	public List<ProductHistoryDataModel> GetProductHistoryByProduct(string productId)
	{
		_logger.LogInformation("GetProductHistoryByProduct for {productId}", productId);
		if (productId.IsEmpty())
		{
			throw new ArgumentNullException(nameof(productId));
		}
		if (!productId.IsGuid())
		{
			throw new ValidationException("The value in the field productId is not a unique identifier.");
		}
		return _productStorageContract.GetHistoryByProductId(productId) ?? throw new NullListException();
	}

	public ProductDataModel GetProductByData(string data)
	{
		_logger.LogInformation("Get element by data: {data}", data);
		if (data.IsEmpty())
		{
			throw new ArgumentNullException(nameof(data));
		}
		if (data.IsGuid())
		{
			return _productStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
		}
		return _productStorageContract.GetElementByName(data) ?? throw new ElementNotFoundException(data);
	}

	public void InsertProduct(ProductDataModel productDataModel)
	{
		_logger.LogInformation("New data: {json}", JsonSerializer.Serialize(productDataModel));
		ArgumentNullException.ThrowIfNull(productDataModel);
		productDataModel.Validate();
		_productStorageContract.AddElement(productDataModel);
	}

	public void UpdateProduct(ProductDataModel productDataModel)
	{
		_logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(productDataModel));
		ArgumentNullException.ThrowIfNull(productDataModel);
		productDataModel.Validate();
		_productStorageContract.UpdElement(productDataModel);
	}

	public void DeleteProduct(string id)
	{
		_logger.LogInformation("Delete by id: {id}", id);
		if (id.IsEmpty())
		{
			throw new ArgumentNullException(nameof(id));
		}
		if (!id.IsGuid())
		{
			throw new ValidationException("Id is not a unique identifier");
		}
		_productStorageContract.DelElement(id);
	}
}
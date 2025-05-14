using BallContratcs.BusinessLogicsContracts;
using BallContratcs.DataModels;
using BallContratcs.Exceptions;
using BallContratcs.Extensions;
using BallContratcs.StoragesContracts;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BallBusinessLogic.Implementations;

internal class PostBusinessLogicContract(IPostStorageContract postStorageContract, ILogger logger) : IPostBusinessLogicContract
{
	private readonly ILogger _logger = logger;
	private readonly IPostStorageContract _postStorageContract = postStorageContract;

	public List<PostDataModel> GetAllPosts(bool onlyActive = true)
	{
		_logger.LogInformation("GetAllPosts params: {onlyActive}", onlyActive);
		return _postStorageContract.GetList(onlyActive) ?? throw new NullListException();
	}

	public List<PostDataModel> GetAllDataOfPost(string postId)
	{
		_logger.LogInformation("GetAllDataOfPost for {postId}", postId);
		if (postId.IsEmpty())
		{
			throw new ArgumentNullException(nameof(postId));
		}
		if (!postId.IsGuid())
		{
			throw new ValidationException("The value in the field postId is not a unique identifier.");
		}
		return _postStorageContract.GetPostWithHistory(postId) ?? throw new NullListException();
	}

	public PostDataModel GetPostByData(string data)
	{
		_logger.LogInformation("Get element by data: {data}", data);
		if (data.IsEmpty())
		{
			throw new ArgumentNullException(nameof(data));
		}
		if (data.IsGuid())
		{
			return _postStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
		}
		return _postStorageContract.GetElementByName(data) ?? throw new ElementNotFoundException(data);
	}

	public void InsertPost(PostDataModel postDataModel)
	{
		_logger.LogInformation("New data: {json}", JsonSerializer.Serialize(postDataModel));
		ArgumentNullException.ThrowIfNull(postDataModel);
		postDataModel.Validate();
		_postStorageContract.AddElement(postDataModel);
	}

	public void UpdatePost(PostDataModel postDataModel)
	{
		_logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(postDataModel));
		ArgumentNullException.ThrowIfNull(postDataModel);
		postDataModel.Validate();
		_postStorageContract.UpdElement(postDataModel);
	}

	public void DeletePost(string id)
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
		_postStorageContract.DelElement(id);
	}

	public void RestorePost(string id)
	{
		_logger.LogInformation("Restore by id: {id}", id);
		if (id.IsEmpty())
		{
			throw new ArgumentNullException(nameof(id));
		}
		if (!id.IsGuid())
		{
			throw new ValidationException("Id is not a unique identifier");
		}
		_postStorageContract.ResElement(id);
	}
}
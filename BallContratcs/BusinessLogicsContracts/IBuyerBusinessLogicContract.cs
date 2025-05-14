using BallContratcs.DataModels;

namespace BallContratcs.BusinessLogicsContracts;

public interface IBuyerBusinessLogicContract
{
	List<BuyerDataModel> GetAllBuyers();

	BuyerDataModel GetBuyerByData(string data);

	void InsertBuyer(BuyerDataModel buyerDataModel);

	void UpdateBuyer(BuyerDataModel buyerDataModel);

	void DeleteBuyer(string id);
}
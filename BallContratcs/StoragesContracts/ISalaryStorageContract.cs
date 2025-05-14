using BallContratcs.DataModels;

namespace BallContratcs.StoragesContracts;

public interface ISalaryStorageContract
{
	List<SalaryDataModel> GetList(DateTime startDate, DateTime endDate, string? workerId = null);

	void AddElement(SalaryDataModel salaryDataModel);
}
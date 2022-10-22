using SalesLiftPOC.Models;

namespace SalesLiftPOC.Interface
{
    public interface IJobsRepository
    {
        Task<IEnumerable<Job>> GetJobsName();
        Task AddData(ActualData data);
        Task AddData(ActualData[] value);
    }
}

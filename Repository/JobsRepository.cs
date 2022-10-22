using Microsoft.EntityFrameworkCore;
using APIEndpointAuthentication.Interface;
using APIEndpointAuthentication.Models;


namespace APIEndpointAuthentication.Repository
{
    public class JobsRepository : IJobsRepository
    {

        private readonly AppDbContext _appDbContext;
        public JobsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Job>> GetJobsName()
        {
            return await _appDbContext.Jobs.ToListAsync();
        }

        public async Task AddData(ActualData data)
        {

            var result = await _appDbContext.ActualData.AddAsync(data);
            await _appDbContext.SaveChangesAsync();

        }
        // Updates list of objects in db
        public async Task AddData(ActualData[] value)
        {
            foreach (ActualData data in value)
            {
                var result = await _appDbContext.ActualData.AddAsync(data);
                await _appDbContext.SaveChangesAsync();

            }
        }
    }
}

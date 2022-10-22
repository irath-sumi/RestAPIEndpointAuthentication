using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using APIEndpointAuthentication.Interface;
using APIEndpointAuthentication.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIEndpointAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobsRepository _jobs;

        private AppDbContext _appDbContext;
        public JobsController(IJobsRepository jobrepository)
        {
            _jobs = jobrepository;
        }

        // GET: api/<JobsController>
        [HttpGet]
       // [Authorize(Policy = "ShouldBeEmployee")] // applying the policy
        // Get the job names from the table
        public async Task<IEnumerable<Job>> GetJobNames()
        {
           // var abc  = await _jobs.GetJobsName();
            return await _jobs.GetJobsName();
        }


        // POST api/<JobsController>
        [HttpPost]
        /// Stores the scrapped data in db
        public async Task Post([FromBody] ActualData[] value)
        {

            await _jobs.AddData(value);

        }
    }
}

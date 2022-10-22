using Microsoft.EntityFrameworkCore;

namespace APIEndpointAuthentication.Models
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
                    }

        public DbSet<ActualData> ActualData { get; set; }

        public DbSet<Job> Jobs { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace RouteToCodePost.Data
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options)
            : base(options)
        {
        }

        public DbSet<Band> Bands { get; set; }
    }
}

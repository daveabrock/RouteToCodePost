using Microsoft.EntityFrameworkCore;

namespace RouteToCodePost.Data
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Band> Bands { get; set; }
    }
}

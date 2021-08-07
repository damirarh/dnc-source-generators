using Microsoft.EntityFrameworkCore;

namespace SrcGenClient.Api.Entities
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}

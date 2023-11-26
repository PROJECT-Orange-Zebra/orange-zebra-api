using Orange.Zebra.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
namespace Orange.Zebra.Data
{
    public class StoreContext: DbContext   
    {
    public StoreContext(DbContextOptions<StoreContext> options)
        :base(options)
        {}
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            DbInitializer.Initialize(builder);
        }

    }
}
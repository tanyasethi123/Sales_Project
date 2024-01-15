using SalesProject.Model;
using Microsoft.EntityFrameworkCore;

namespace SalesProject
{
    public class AppDbContext: DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
        }

        public DbSet<SalesModel> sales_fact { get; set; }
        public DbSet<CustomerModel> supermarket_customer { get; set; }
        public DbSet<LocationModel> supermarket_location { get; set; }
        public DbSet<DateModel> supermarket_date { get; set; }
        public DbSet<ProductModel> supermarket_product { get; set; }
        public DbSet<Identity> DemoTable { get; set; }
	}
}


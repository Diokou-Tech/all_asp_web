using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DutchTreat.Data
{
	public class DBContextDutch :DbContext 
	{
		private readonly IConfiguration _config;
		public DBContextDutch(IConfiguration config)
		{
			this._config = config;
		}
		// definir les tables
		public DbSet<Product> Products{ get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			//optionsBuilder.UseSqlServer(_config.GetConnectionString("DbDutch"));
			optionsBuilder.UseSqlServer(_config["ConnectionStrings:DbDutch"]);
		}
	}
}

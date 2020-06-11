using Microsoft.EntityFrameworkCore;


namespace FreeAgentBrowser.Models
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(
			DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Player> Players { get; set; }
		public DbSet<Position> Positions { get; set; }

		protected override void OnModelCreating(
			ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//seed positions
			modelBuilder.Entity<Position>().
				HasData(
					new Position
					{
						PositionId = 1,
						PositionName = "QB",
						Description = "Quarterback"
					});

		}
	}
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GravityVectorToolKit.Tools.AisCombine.DataAccess
{
	/// <summary>
	/// Created using the following command: 
	///		dotnet ef dbcontext scaffold "Data Source=E:\\madart-weather-2018.sqlite" Microsoft.EntityFrameworkCore.Sqlite -t weather_hist
	/// </summary>
	public partial class MadartWeatherDbContext : DbContext
	{
		private string databasePath;

		public MadartWeatherDbContext(string databasePath)
		{
			this.databasePath = databasePath;
		}

		public MadartWeatherDbContext(DbContextOptions<MadartWeatherDbContext> options)
			: base(options)
		{
		}

		public virtual DbSet<WeatherHist> WeatherHists { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlite("Data Source=" + databasePath);
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<WeatherHist>(entity =>
			{
				entity.HasNoKey();

				entity.ToTable("weather_hist");

				entity.Property(e => e.Dd)
					.HasColumnType("FLOAT")
					.HasColumnName("dd");

				entity.Property(e => e.Ff)
					.HasColumnType("FLOAT")
					.HasColumnName("ff");

				entity.Property(e => e.Geohash)
				.HasColumnType("VARCHAR(10)")
				.HasColumnName("geohash");

				entity.Property(e => e.Hs)
					.HasColumnType("FLOAT")
					.HasColumnName("hs");

				entity.Property(e => e.Thq)
					.HasColumnType("FLOAT")
					.HasColumnName("thq");

				entity.Property(e => e.Epoch)
					.HasColumnType("DOUBLE")
					.HasColumnName("epoch");
			});


			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}

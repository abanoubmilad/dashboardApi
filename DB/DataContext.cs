using DashboardApi.Entities;
using DashboardApi.Entities.Data;
using Microsoft.EntityFrameworkCore;

namespace DashboardApi.Helpers
{
	public class DataContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// init here instead of startup.cs 
			// to support Design-time DbContext Creation for migrations
			var appSettings = AppSettings.GetAppSettings();
			optionsBuilder.UseMySQL(@appSettings.DBServerConnectionString + @appSettings.DashBoardDBConnectionString);
		}
		public DbSet<DashboardUser> DashboardUsers { get; set; }
		public DbSet<DashboardInvitation> DashboardInvitations { get; set; }

		public DbSet<Tag> Tags { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<Lesson> Lessons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// DashboardInvitation
			modelBuilder.Entity<DashboardInvitation>()
			.HasKey(o => new { o.Email });

			// Item
			modelBuilder.Entity<Item>()
			.HasIndex(o => new { o.ItemName });
		
			// Lesson
			modelBuilder.Entity<Lesson>()
			.HasIndex(o => new { o.LessonName });

			// Tag
			modelBuilder.Entity<Tag>()
			.HasIndex(o => new { o.TagName });
			modelBuilder.Entity<Tag>()
			.HasIndex(o => new { o.TagType });

			// Category
			modelBuilder.Entity<Category>()
			.HasIndex(o => new { o.CategoryName });

			// relations

			// Category and Lesson
			modelBuilder.Entity<LessonCategory>()
			 .HasKey(o => new { o.LessonId, o.CategoryId });

			modelBuilder.Entity<LessonCategory>()
				.HasOne(o => o.Lesson)
				.WithMany(o => o.LessonCategories)
				.HasForeignKey(o => o.LessonId);

			modelBuilder.Entity<LessonCategory>()
			.HasOne(o => o.Category)
			.WithMany(o => o.LessonCategories)
			.HasForeignKey(o => o.CategoryId);


			// Item and Lesson
			modelBuilder.Entity<LessonItem>()
			.HasKey(o => new { o.LessonId, o.ItemId });

			modelBuilder.Entity<LessonItem>()
				.HasOne(o => o.Lesson)
				.WithMany(o => o.LessonItems)
				.HasForeignKey(o => o.LessonId);

			modelBuilder.Entity<LessonItem>()
			.HasOne(o => o.Item)
			.WithMany(o => o.LessonItems)
			.HasForeignKey(o => o.ItemId);


			// Item and Tag
			modelBuilder.Entity<ItemTag>()
			.HasKey(o => new { o.TagId, o.ItemId });

			modelBuilder.Entity<ItemTag>()
				.HasOne(o => o.Tag)
				.WithMany(o => o.ItemTags)
				.HasForeignKey(o => o.TagId);

			modelBuilder.Entity<ItemTag>()
			.HasOne(o => o.Item)
			.WithMany(o => o.ItemTags)
			.HasForeignKey(o => o.ItemId);

		}
	}
}
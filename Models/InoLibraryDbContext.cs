using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace InoLibrary.Models
{
    public class InoLibraryDbContext : IdentityDbContext<User>
    {
        public InoLibraryDbContext(DbContextOptions<InoLibraryDbContext> options) :base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PublicationCategory>()
                .HasKey(t => new { t.PublicationId, t.CategoryId });

            modelBuilder.Entity<PublicationCategory>()
                .HasOne(pc => pc.Publication)
                .WithMany(p => p.PublicationCategories)
                .HasForeignKey(pc => pc.PublicationId);

            modelBuilder.Entity<PublicationCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.PublicationCategories)
                .HasForeignKey(pc => pc.CategoryId);
        }
        public DbSet<PublicationCategory> PublicationCategories { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

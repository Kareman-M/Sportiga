using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sportiga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sportiga.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Articles> Articles { get; set; }
        public DbSet<ContactUs> Contacts { get; set; }
        //public DbSet<ArticleWriter> ArticleWriters { get; set; }
        public DbSet<Keywords> Keywords { get; set; }
        public DbSet<Socialmedia> Socialmedias { get; set; }

        public object ApplicationUser { get; set; }

        public DbSet<Sportiga.Models.ProjectRole> ProjectRole { get; set; }
    }

}

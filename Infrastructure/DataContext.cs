using Core.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
   public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // cuz the id from GUID type we must write it in Sql Qury to generate new value each time 
            // if we put the id as integer we dont need this proceure
            modelBuilder.Entity<Owner>().Property(x => x.Id).HasDefaultValueSql("NEWID()"); // NEWWID => its funcion in sql give new id for the property 
            modelBuilder.Entity<PortfolioItem>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

            // Seed Data
            modelBuilder.Entity<Owner>().HasData(
                new Owner()
                {
                    Id = Guid.NewGuid(),
                    FullName ="Abdallah Hassnat",
                    Avatar = "1.jpg",
                    Profil = "Full Stack .Net Developer"
                }
                );
        }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
    }
}

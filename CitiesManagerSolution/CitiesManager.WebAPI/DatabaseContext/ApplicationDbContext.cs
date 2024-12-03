﻿using CitiesManager.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public ApplicationDbContext() { }

        public virtual DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<City>().HasData(new City() { CityID = Guid.Parse("C5662E16-B261-4EED-AD28-40F13DBF3434"), CityName="New York" });
            modelBuilder.Entity<City>().HasData(new City() { CityID = Guid.Parse("7B1E5136-C33C-45DF-8754-1C4B3E197F40"), CityName="Delhi" });
        }
    }
}
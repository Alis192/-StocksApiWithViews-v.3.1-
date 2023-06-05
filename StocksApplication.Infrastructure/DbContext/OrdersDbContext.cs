using System;
using System.Collections.Generic;
using Entities;
using Microsoft.EntityFrameworkCore;


namespace Entities
{
    public class OrdersDbContext : DbContext
    {

        public OrdersDbContext(DbContextOptions options) : base(options) //whatever options is supplied in program.cs it will be supplied to the constructor of DbContext class as options parameter
        {

        }


        public DbSet<BuyOrder> BuyOrders { get; set; } //each DbSet represents a database table
        public DbSet<SellOrder> SellOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //binding dbsets to corresponding tables
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");
        }
    }
}

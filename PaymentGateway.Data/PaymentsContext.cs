using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Data
{
    public class PaymentsContext : DbContext
    {
        public PaymentsContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Payment> Payments { get; private set; }
        public DbSet<Merchant> Merchants { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("DECIMAL(19, 4)");

            modelBuilder.Entity<Merchant>();

        }

    }
}

using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Data
{
    public class PaymentsContext : DbContext
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Payment>();
            modelBuilder.Entity<Merchant>();

        }

    }
}

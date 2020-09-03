using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace TaxManagementSystem.Models
{
    public partial class TaxDbContex : DbContext
    {

        public TaxDbContex() 
        { }
        public DbSet<TaxRegistration> TaxRegistrations { get; set; }

        public DbSet<ApplyForTax> ApplyForTaxes { get; set; }

        public DbSet<AdminUser> AdminUsers { get; set; }

        public DbSet<ContactInfo> contactInfos { get; set; }

        public DbSet<PaymentRecord> PaymentRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxRegistration>()
            .HasOptional<ApplyForTax>(s => s.ApplyForTax)
            .WithMany()
            .WillCascadeOnDelete(false);
        }

    }




}
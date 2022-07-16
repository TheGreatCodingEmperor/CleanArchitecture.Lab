using DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;

namespace Application.Models;

public class MyContext : DbContext {
    public MyContext (DbContextOptions<MyContext> options) : base (options) { }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<InvoiceEntity> Invoices { get; set; }
    public DbSet<InvoiceItemEntity> InvoiceItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.LogTo(Console.WriteLine);

    protected override void OnModelCreating (ModelBuilder modelBuilder) {
        modelBuilder.Entity<CustomerEntity> (order => {
            var orderNumber = order.Property (p => p.CustomerId);
            orderNumber.ValueGeneratedOnAdd ();
            // only for in-memory
            // if (Database.IsInMemory ())
            //     orderNumber.HasValueGenerator<InMemoryIntegerValueGenerator<int>> ();
        }).Entity<InvoiceEntity>(order =>{
            var orderNumber = order.Property (p => p.Id);
            orderNumber.ValueGeneratedOnAdd ();
        }).Entity<InvoiceItemEntity>(order =>{
            var orderNumber = order.Property (p => p.InvoiceItemId);
            orderNumber.ValueGeneratedOnAdd ();
        });
    }
}